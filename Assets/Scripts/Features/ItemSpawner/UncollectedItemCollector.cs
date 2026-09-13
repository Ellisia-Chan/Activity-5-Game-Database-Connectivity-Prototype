using UnityEngine;
using Core.Enums;
using Features.Item;


namespace Features.ItemSpawnerSystem {
    /// <summary>
    /// Handles the collection of uncollected items.
    /// </summary>
	public class UncollectedItemCollector : MonoBehaviour {
        /// <summary>
        /// Handles the collection of uncollected items.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.TryGetComponent<ICollectible>(out ICollectible collectible)) {
                collectible.ReturnToSpawner();
            }
        }
    }
}