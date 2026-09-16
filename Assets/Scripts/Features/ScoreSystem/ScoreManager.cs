using UnityEngine;
using Core.EventSystem;
using Core.Events.PlayerSystem;
using Core.Events.ScoreSystem;
using Core.ServiceLocator;

namespace Features.ScoreSystem {
    /// <summary>
    /// Handles the score system
    /// </summary>
	public class ScoreManager : MonoBehaviour, IScoreSystem {

        // --- Private Properties ---
		private int score = 0;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (ServiceRegistry.IsRegistered<IScoreSystem>()) {
                Destroy(gameObject);
                return;
            } else {
                ServiceRegistry.Register<IScoreSystem>(this);
            }
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnItemCollected>(OnItemCollected);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnItemCollected>(OnItemCollected);
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<IScoreSystem>(this);
        }

        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        /// <summary>
        /// Event handler for when an item is collected
        /// </summary>
        /// <param name="evt"></param>
        private void OnItemCollected(Evt_OnItemCollected evt) {
            score += evt.ItemScore;

            EventBus.Publish(new Evt_OnScoreUpdate(score));
        }

        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        /// <summary>
        /// Returns the final score
        /// </summary>
        /// <returns></returns>
        public int GetFinalScore() => score;

    }
}