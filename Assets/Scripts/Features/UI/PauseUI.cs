using Core.Enums.GameSystem;
using Core.Events.GameSystem;
using Core.Events.UI;
using Core.EventSystem;
using Core.ServiceLocator;
using Core.Enums.SceneSystem;

using Editor;
using UnityEngine;

namespace Features.UI {
    /// <summary>
    /// Handles updating the pause UI
    /// </summary>
    public class PauseUI : MonoBehaviour {
        [Header("States")]
        [SingleSelectionFlag]
        [SerializeField] private GameState showState = GameState.Paused;

        [Header("UI Panels")]
        [SerializeField] private GameObject panel;

        // --- Service Dependencies ---
        private ISceneSystem sceneSystemService;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            panel.SetActive(false);
        }

        private void Start() {
            sceneSystemService = ServiceRegistry.Get<ISceneSystem>();
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            if ((evt.NewState & showState) != 0) {
                TogglePanel(true);
            }
            else {
                TogglePanel(false);
            }
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        private void TogglePanel(bool state) {
            panel.SetActive(state);
        }

        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        public void ResumePause() {
            EventBus.Publish(new Evt_OnResumeButtonAction());
        }

        public void MainMenu() {
            sceneSystemService.LoadScene(SceneID.MainMenuScene, SceneID.LoadingScene, showLoadingScreen: true, isAsync: true);
        }
    }
}