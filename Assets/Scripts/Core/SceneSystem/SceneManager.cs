using Core.Enums.SceneSystem;
using Core.Events.SceneSystem;
using Core.EventSystem;
using Core.ServiceLocator;

using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Core.SceneSystem {
    /// <summary>
    /// Manages the loading and unloading of scenes.
    /// </summary>
    public class SceneManager : MonoBehaviour, ISceneSystem {
        [Header("Settings")]
        [SerializeField] private float loadingScenePostDelay = 1f;
        // =====================================================================
        //
        //                          Private Fields
        //
        // =====================================================================
        private bool _isLoading = false;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (ServiceRegistry.IsRegistered<ISceneSystem>()) {
                Destroy(gameObject);
                return;
            }
            else {
                ServiceRegistry.Register<ISceneSystem>(this);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<ISceneSystem>(this);
        }


        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        /// <summary>
        /// Loads a scene.
        /// </summary>
        /// <param name="targetSceneID"></param>
        /// <param name="loadingScreenID"></param>
        /// <param name="showLoadingScreen"></param>
        /// <param name="isAdditive"></param>
        /// <param name="isAsync"></param>
        /// <param name="unloadCurrentFirst"></param>
        /// <param name="sceneToUnload"></param>
        public void LoadScene(
            SceneID targetSceneID,
            SceneID loadingScreenID = SceneID.None,
            bool showLoadingScreen = false,
            bool isAdditive = false,
            bool isAsync = false,
            bool unloadCurrentFirst = false,
            SceneID sceneToUnload = SceneID.None
            ) {

            if (targetSceneID == SceneID.None) {
                Debug.LogWarning("[SceneManager] Cannot load scene: target SceneID is None.");
                return;
            }

            if (_isLoading) {
                Debug.LogWarning(
                    $"[SceneManager] Cannot load scene '{targetSceneID}': " +
                    "a scene load is already in progress."
                );
                return;
            }

            // A loading screen only has a chance to actually be seen if the
            // load happens asynchronously (see LoadSceneAsync's allowSceneActivation
            // gating). A synchronous load blocks the main thread until the target
            // scene is ready, so the loading screen would load and unload within
            // the same frame stall. Rather than silently loading a loading screen
            // that's never seen, force the async path whenever one is requested.
            if (showLoadingScreen && !isAsync) {
                Debug.LogWarning(
                    $"[SceneManager] showLoadingScreen was requested for '{targetSceneID}' " +
                    "with isAsync = false. Forcing async load, since a synchronous load " +
                    "would never actually display the loading screen."
                );
                isAsync = true;
            }

            EventBus.Publish(new Evt_OnSceneLoadStarted(targetSceneID));

            string targetSceneName = targetSceneID.ToString();

            LoadSceneMode mode = isAdditive
                ? LoadSceneMode.Additive
                : LoadSceneMode.Single;

            if (isAsync) {
                _isLoading = true;
                StartCoroutine(LoadSceneAsync(
                    targetSceneID,
                    loadingScreenID,
                    showLoadingScreen,
                    mode,
                    unloadCurrentFirst,
                    sceneToUnload
                ));
            }
            else {
                UnitySceneManager.LoadScene(
                    targetSceneName,
                    mode
                );
            }
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="targetSceneID"></param>
        /// <param name="loadingScreenID"></param>
        /// <param name="showLoadingScreen"></param>
        /// <param name="mode"></param>
        /// <param name="unloadCurrentFirst"></param>
        /// <param name="sceneToUnload"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(
            SceneID targetSceneID,
            SceneID loadingScreenID,
            bool showLoadingScreen,
            LoadSceneMode mode,
            bool unloadCurrentFirst,
            SceneID sceneToUnload
            ) {

            string targetSceneName = targetSceneID.ToString();
            Scene loadingScreenScene = default;

            if (showLoadingScreen && loadingScreenID != SceneID.None) {
                // Match the target scene's mode: if the target is loading as
                // Single (replacing everything), the loading screen should
                // become the root scene too, rather than stacking additively
                // on top of whatever scene triggered this load. If the target
                // is Additive, the loading screen stays additive so it can
                // layer on top and be cleanly unloaded afterward.
                LoadSceneMode loadingScreenMode = mode == LoadSceneMode.Single
                    ? LoadSceneMode.Single
                    : LoadSceneMode.Additive;

                yield return UnitySceneManager.LoadSceneAsync(
                    loadingScreenID.ToString(),
                    loadingScreenMode
                );
                loadingScreenScene = UnitySceneManager.GetSceneByName(loadingScreenID.ToString());

                if (loadingScreenScene.IsValid()) {
                    UnitySceneManager.SetActiveScene(loadingScreenScene);
                }
            }

            if (unloadCurrentFirst && mode == LoadSceneMode.Additive) {
                // If a specific scene was named, unload that one rather than
                // guessing based on GetActiveScene() — with multiple additive
                // scenes loaded (hub, UI overlay, sub-areas, etc.), "the active
                // scene" isn't necessarily the one the caller means to swap out.
                Scene sceneToUnloadInstance = sceneToUnload != SceneID.None
                    ? UnitySceneManager.GetSceneByName(sceneToUnload.ToString())
                    : UnitySceneManager.GetActiveScene();

                if (sceneToUnloadInstance.IsValid() && sceneToUnloadInstance != loadingScreenScene) {
                    AsyncOperation unloadOperation =
                        UnitySceneManager.UnloadSceneAsync(sceneToUnloadInstance);

                    yield return unloadOperation;
                }
                else if (sceneToUnload != SceneID.None && !sceneToUnloadInstance.IsValid()) {
                    Debug.LogWarning(
                        $"[SceneManager] unloadCurrentFirst was set with sceneToUnload = " +
                        $"'{sceneToUnload}', but no loaded scene by that name was found."
                    );
                }
            }

            AsyncOperation loadOperation = UnitySceneManager.LoadSceneAsync(targetSceneName, mode);

            if (loadOperation == null) {
                Debug.LogError(
                    $"[SceneManager] Failed to start loading scene '{targetSceneName}'. " +
                    "Check that it's added to Build Settings and the name matches exactly."
                );

                if (loadingScreenScene.IsValid()) {
                    yield return UnitySceneManager.UnloadSceneAsync(loadingScreenScene);
                }

                _isLoading = false;
                yield break;
            }

            loadOperation.allowSceneActivation = false;

            while (loadOperation.progress < 0.9f) {
                float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                EventBus.Publish(new Evt_OnSceneLoadProgress(progress));
                yield return null;
            }

            EventBus.Publish(new Evt_OnSceneLoadProgress(1f));

            if (loadingScenePostDelay > 0f) {
                // Realtime, not scaled time — scene transitions (e.g. leaving
                // a paused game where Time.timeScale is 0) must not be gated
                // by whatever the game's current timeScale happens to be.
                yield return new WaitForSecondsRealtime(loadingScenePostDelay);
            }

            loadOperation.allowSceneActivation = true;
            yield return loadOperation;

            if (mode == LoadSceneMode.Additive) {
                Scene loadedScene =
                    UnitySceneManager.GetSceneByName(targetSceneName);

                if (loadedScene.IsValid()) {
                    UnitySceneManager.SetActiveScene(loadedScene);
                }
            }

            if (loadingScreenScene.IsValid()) {
                yield return UnitySceneManager.UnloadSceneAsync(loadingScreenScene);
            }

            _isLoading = false;

            EventBus.Publish(new Evt_OnSceneLoadCompleted(targetSceneID));
        }
    }
}