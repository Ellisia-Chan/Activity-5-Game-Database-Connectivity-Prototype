using Core.Enums.GameSystem;
using Core.Events.GameSystem;
using Core.Events.UI;
using Core.EventSystem;
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


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            panel.SetActive(false);
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
    }
}