using Core.Enums.GameSystem;
using Core.Events.GameSystem;
using Core.EventSystem;
using Core.ServiceLocator;
using Editor;
using TMPro;
using UnityEngine;

namespace Features.UI {
    public class GameOverUI : MonoBehaviour {
        [Header("States")]
        [SingleSelectionFlag]
        [SerializeField] private GameState showState = GameState.Over;

        [Header("UI Panels")]
        [SerializeField] private GameObject panel;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;

        // --- Service Dependencies ---
        private IScoreSystem socreSystemService;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            panel.SetActive(false);
        }

        private void Start() {
            socreSystemService = ServiceRegistry.Get<IScoreSystem>();

            scoreText.text = "0";
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
                HandleGameOverUI();
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
        /// <summary>
        /// Hides the panel
        /// </summary>
        /// <param name="state"></param>
        private void TogglePanel(bool state) {
            panel.SetActive(state);
        }

        private void HandleGameOverUI() {
            scoreText.text = socreSystemService.GetFinalScore().ToString("N0");
        }

        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        /// <summary>
        /// Restarts the game invoke by the retry button
        /// </summary>
        public void RetryGame() { }

        /// <summary>
        /// Returns to the main menu invoke by the main menu button
        /// </summary>
        public void MainMenu() { }
    }
}