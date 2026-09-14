using UnityEngine;

namespace Core.Events.PlayerSystem {
    /// <summary>
    /// Event for player item collection
    /// </summary>
    public readonly struct Evt_OnItemCollected {
        public readonly int ItemScore;

        public Evt_OnItemCollected(int itemScore) {
            ItemScore = itemScore;
        }
    }

    /// <summary>
    /// Event for player skill
    /// </summary>
    public readonly struct Evt_OnSkillPointsUpdated {
        public readonly float CurrentSkillPoints;
        public readonly float MaxSkillPoints;

        public Evt_OnSkillPointsUpdated(float currentSkillPoints, float maxSkillPoints) {
            CurrentSkillPoints = currentSkillPoints;
            MaxSkillPoints = maxSkillPoints;
        }
    }

    /// <summary>
    /// Event for player skill activation
    /// </summary>
    public readonly struct Evt_OnSkillActivated { }

    /// <summary>
    /// Event for player skill deactivation
    /// </summary>
    public readonly struct Evt_OnSkillDeactivated { }

    /// <summary>
    /// Event for player skill timer update
    /// </summary>
    public readonly struct Evt_OnSkillTimerUpdated {
        public readonly float RemainingTime;
        public readonly float MaxTime;

        public Evt_OnSkillTimerUpdated(float remainingTime, float maxTime) {
            RemainingTime = remainingTime;
            MaxTime = maxTime;
        }
    }

}
