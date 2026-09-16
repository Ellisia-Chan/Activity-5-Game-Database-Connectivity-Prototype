using UnityEngine;
using UnityEngine.UI;

using Core.EventSystem;
using Core.Events.SceneSystem;

namespace Features.UI {
    /// <summary>
    /// Handles updating the loading UI
    /// </summary>
    public class LoadingUI : MonoBehaviour {
        [Header("UI Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Slider progressBar;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            panel.SetActive(true);
            progressBar.value = 0;
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnSceneLoadProgress>(OnSceneLoadProgress);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnSceneLoadProgress>(OnSceneLoadProgress);
        }

        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnSceneLoadProgress(Evt_OnSceneLoadProgress evt) {
            progressBar.value = evt.Progress;
        }
    }
}