using Core.PoolSystem;
using UnityEngine;

namespace Features.Item {
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemDataSO : ScriptableObject {
        public PoolItemSO PoolItemSO;
        [Range(0f, 1f)] public float Probability;
        public int Score;
    }
}