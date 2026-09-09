using UnityEngine;
using Core.PoolSystem;
using Core.ServiceLocator;

namespace Features.ItemSpawner {
	/// <summary>
	/// ItemData
	/// </summary>
	[System.Serializable]
	public class ItemData {
		public PoolItemSO poolItemSO;
		[Range(0f, 1f)] public float probability;
	}

	/// <summary>
	/// ItemSpawner 
	/// </summary>
	public class ItemSpawner : MonoBehaviour {
		[Header("ItemData")]
        [SerializeField] private ItemData[] itemData;

		[Header("Spawnpoints")]
		[SerializeField] private Transform[] spawnPoints;

		// --- Service Dependencies ---
		private IPoolSystem poolSystemService;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            poolSystemService = ServiceRegistry.Get<IPoolSystem>();
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
    }
}