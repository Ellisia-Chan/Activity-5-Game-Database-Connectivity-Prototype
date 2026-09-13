using UnityEngine;
using TMPro;
using Core.ServiceLocator;

namespace Features.UI {
    /// <summary>
    /// Handles updating the timer UI
    /// </summary>
    public class TimerUI : MonoBehaviour {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI timerText;

        // --- Service Dependencies ---
        private IGameSystem gameSystemService;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            gameSystemService = ServiceRegistry.Get<IGameSystem>();
        }

        private void Update() {
            HandleTimer();
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Handles updating the timer
        /// </summary>
        private void HandleTimer() {
            float time = gameSystemService.RemainingTime;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}