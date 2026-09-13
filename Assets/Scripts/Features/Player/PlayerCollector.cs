using Core.Events.PlayerSystem;
using Core.EventSystem;
using Features.Item;
using UnityEngine;

namespace Features.Player {
    /// <summary>
    /// Handles player item collection
    /// </summary>
    public class PlayerCollector : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {
            // Check if the object is collectible
            if (collision.gameObject.TryGetComponent<ICollectible>(out var collectible)) {
                collectible.ReturnToSpawner();

                EventBus.Publish(new Evt_OnItemCollected(collectible.ItemData.Score));
            }
        }
    }
}