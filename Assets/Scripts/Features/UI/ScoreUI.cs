using UnityEngine;
using TMPro;

using Core.EventSystem;
using Core.Events.ScoreSystem;

namespace Features.UI {
	public class ScoreUI : MonoBehaviour {
		[Header("References")]
		[SerializeField] private TextMeshProUGUI scoreText;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            scoreText.text = "0";
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnScoreUpdate>(OnScoreUpdate);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnScoreUpdate>(OnScoreUpdate);
        }

        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnScoreUpdate(Evt_OnScoreUpdate evt) {
            UpdateScoreText(evt.Score);
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        private void UpdateScoreText(int score) {
            scoreText.text = score.ToString("N0");
        }
    }
}