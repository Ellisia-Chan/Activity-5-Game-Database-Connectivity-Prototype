using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;
using Core.Events.PlayerSystem;

namespace Features.UI {
    /// <summary>
    /// Handles updating the skill UI
    /// </summary>
	public class SkillUI : MonoBehaviour {
		[Header("References")]
		[SerializeField] private Image slicedImage;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            slicedImage.fillAmount = 0f;
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnSkillPointsUpdated>(OnSkillPointsUpdated);
            EventBus.Subscribe<Evt_OnSkillTimerUpdated>(OnSkillTimerUpdated);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnSkillPointsUpdated>(OnSkillPointsUpdated);
            EventBus.Unsubscribe<Evt_OnSkillTimerUpdated>(OnSkillTimerUpdated);
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnSkillPointsUpdated(Evt_OnSkillPointsUpdated evt) {
            slicedImage.fillAmount = evt.CurrentSkillPoints / evt.MaxSkillPoints;
        }


        private void OnSkillTimerUpdated(Evt_OnSkillTimerUpdated evt) {
            float progress = evt.RemainingTime / evt.MaxTime;
            slicedImage.fillAmount = progress;
        }
    }
}
