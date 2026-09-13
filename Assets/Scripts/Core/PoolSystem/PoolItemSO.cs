using UnityEngine;

namespace Core.PoolSystem {
    /// <summary>
    /// A scriptable object that holds information about a pool. Used by PoolRuntimeSystem to create object pools.
    /// </summary>
    [CreateAssetMenu(fileName = "PoolItem", menuName = "ScriptableObjects/PoolItem")]
    public class PoolItemSO : ScriptableObject {
        [HideInInspector] public string itemName;
        public GameObject prefab;
        public int poolSize;
        public Vector2 resetPosition = new Vector2(0, -100f);

        /// <summary>
        /// Assigns itemName to the name of the object if it's empty.
        /// </summary>
        private void OnValidate() {
            if (string.IsNullOrEmpty(itemName)) {
                itemName = name;
            }
        }
    }
}