using UnityEngine;
using Core.PoolSystem;

namespace Features.Item {
    /// <summary>
    /// The item data.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemDataSO : ScriptableObject {
        public PoolItemSO PoolItemSO;
        [Range(0f, 1f)] public float Probability;
        public int Score;
    }
}