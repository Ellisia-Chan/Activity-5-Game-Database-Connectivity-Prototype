using UnityEngine;
using Core.EventSystem;
using Core.Events.PlayerSystem;
using Core.Events.InputSystem;

namespace Features.Player {
    /// <summary>
    /// Handles player skill
    /// </summary>
    public class PlayerSkill : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int skillPointsPerItem = 10;
        [SerializeField] private float maxSkillProgress = 100f;
        [SerializeField] private float skillDuration = 8f;

        // --- Private Properties ---
        private float skillProgress = 0f;
        private float skillTimer = 0f;
        private bool isSkillActive = false;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void OnEnable() {
            EventBus.Subscribe<Evt_OnItemCollected>(OnItemCollected);
            EventBus.Subscribe<Evt_OnSkillPerformed>(OnSkillPerformed);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnItemCollected>(OnItemCollected);
            EventBus.Unsubscribe<Evt_OnSkillPerformed>(OnSkillPerformed);
        }

        private void Update() {
            HandleSkillTimer();
        }

        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnItemCollected(Evt_OnItemCollected evt) {
            AddSkillPoints();
        }

        private void OnSkillPerformed(Evt_OnSkillPerformed evt) {
            ActivateSkill();
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Handles skill timer
        /// </summary>
        private void HandleSkillTimer() {
            if (!isSkillActive) return;

            skillTimer -= Time.deltaTime;
            skillTimer = Mathf.Max(skillTimer, 0f);

            EventBus.Publish(new Evt_OnSkillTimerUpdated(skillTimer, skillDuration));

            if (skillTimer <= 0f) {
                skillTimer = 0f;
                isSkillActive = false;

                EventBus.Publish(new Evt_OnSkillDeactivated());
            }
        }

        /// <summary>
        /// Adds skill points
        /// </summary>
        private void AddSkillPoints() {
            if (isSkillActive) return;
            if (skillProgress >= maxSkillProgress) return;

            skillProgress += skillPointsPerItem;
            skillProgress = Mathf.Min(skillProgress, maxSkillProgress);

            EventBus.Publish(new Evt_OnSkillPointsUpdated(skillProgress, maxSkillProgress));
        }

        /// <summary>
        /// Activates skill
        /// </summary>
        private void ActivateSkill() {
            if (skillProgress < maxSkillProgress) return;

            skillProgress = 0f;
            skillTimer = skillDuration;
            isSkillActive = true;

            EventBus.Publish(new Evt_OnSkillActivated());
        }
    }
}