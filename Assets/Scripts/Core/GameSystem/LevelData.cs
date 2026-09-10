using UnityEngine;

namespace Core.GameSystem {
    [System.Serializable]
    public class LevelData {
        public int levelID = 0;

        [Header("Timeline (Seconds)")]
        public float levelDuration = 60f;
        [Tooltip("Remaining time threshold that triggers frenzy mode")]
        public float frenzyTimeFrame = 20f;

        [Header("Spawn Rates (Spawns Per Second)")]
        [Tooltip("Items spawned per second at 00:00")]
        public float startSpawnRate = 1.0f;
        [Tooltip("Items spawned per second right before frenzy begins")]
        public float maxNormalSpawnRate = 2.5f;

        [Header("Frenzy")]
        [Tooltip("Multiplies the current spawn frequency by this factor")]
        public float frenzyMultiplier = 2.0f;

        /// <summary>
        /// Evaluates the current spawn interval (in seconds) given elapsed level time.
        /// </summary>
        public float GetSpawnInterval(float elapsedTime) {
            float remainingTime = Mathf.Max(0f, levelDuration - elapsedTime);
            float normalPhaseDuration = Mathf.Max(0.01f, levelDuration - frenzyTimeFrame);

            // Progress through the normal phase: 0.0 to 1.0
            float normalProgress = Mathf.Clamp01(elapsedTime / normalPhaseDuration);

            // Items per second
            float currentRate = Mathf.Lerp(startSpawnRate, maxNormalSpawnRate, normalProgress);

            // Apply 2x during frenzy
            if (remainingTime <= frenzyTimeFrame) {
                currentRate *= frenzyMultiplier;
            }

            // Convert frequency (Hz) to interval (seconds delay)
            return 1f / Mathf.Max(0.01f, currentRate);
        }
    }
}