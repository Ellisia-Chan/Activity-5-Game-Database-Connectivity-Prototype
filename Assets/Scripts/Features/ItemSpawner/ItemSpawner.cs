using UnityEngine;
using Core.PoolSystem;
using Core.ServiceLocator;
using Core.GameSystem;

namespace Features.ItemSpawner {
    [System.Serializable]
    public class ItemData {
        public PoolItemSO poolItemSO;
        [Range(0f, 1f)] public float probability;
        public int score;
    }

    public class ItemSpawner : MonoBehaviour {
        [Header("Configuration")]
        [SerializeField] private LevelData levelData;

        [Header("ItemData")]
        [SerializeField] private ItemData[] itemData;

        [Header("Spawnpoints")]
        [SerializeField] private Transform[] spawnPoints;

        // --- Service Dependencies ---
        private IPoolSystem poolSystemService;


        // --- Runtime State ---
        private float _elapsedLevelTime = 0f;
        private float _spawnCooldown;
        private bool _isSpawningActive = false;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            poolSystemService = ServiceRegistry.Get<IPoolSystem>();

            // Initialize first spawn cooldown
            _spawnCooldown = levelData.GetSpawnInterval(0f);
        }

        private void Update() {
            InitializeSpawning();
        }



        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        private void InitializeSpawning() {
            if (!_isSpawningActive) return;

            _elapsedLevelTime += Time.deltaTime;

            // Stop spawning when the level finishes
            if (_elapsedLevelTime >= levelData.levelDuration) {
                _isSpawningActive = false;
                return;
            }

            _spawnCooldown -= Time.deltaTime;

            if (_spawnCooldown <= 0f) {
                Spawn();
                // Fetch next cooldown based on the updated time
                _spawnCooldown = levelData.GetSpawnInterval(_elapsedLevelTime);
            }
        }

        /// <summary>
        /// Spawns a random item at a random spawn point.
        /// </summary>
        private void Spawn() {
            if (itemData == null || itemData.Length == 0) return;
            if (spawnPoints == null || spawnPoints.Length == 0) return;

            ItemData selectedItem = GetRandomItemByWeight();
            if (selectedItem?.poolItemSO == null) return;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Retrieve from your pool service at target position/rotation
            poolSystemService.SpawnFromPool(selectedItem.poolItemSO.name, spawnPoint.position, spawnPoint.rotation);
        }

        /// <summary>
        /// Selects an item using standard roulette-wheel weighted probability.
        /// </summary>
        private ItemData GetRandomItemByWeight() {
            float totalWeight = 0f;
            for (int i = 0; i < itemData.Length; i++) {
                totalWeight += itemData[i].probability;
            }

            if (totalWeight <= 0f) {
                return itemData[Random.Range(0, itemData.Length)];
            }

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < itemData.Length; i++) {
                cumulative += itemData[i].probability;
                if (roll <= cumulative) {
                    return itemData[i];
                }
            }

            return itemData[itemData.Length - 1];
        }
    }
}