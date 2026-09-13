using Core.Enums;
using Core.Events.GameSystem;
using Core.EventSystem;
using Core.ServiceLocator;
using Features.Item;
using UnityEngine;

namespace Features.ItemSpawnerSystem {
    /// <summary>
    /// Spawns items at random locations.
    /// </summary>
    public class ItemSpawner : MonoBehaviour {
        [Header("ItemData")]
        [SerializeField] private ItemDataSO[] itemData;

        [Header("Spawnpoints")]
        [SerializeField] private Transform[] spawnPoints;

        // --- Service Dependencies ---
        private IPoolSystem poolSystemService;
        private IGameSystem gameSystemService;


        // --- Runtime State ---
        private float _spawnCooldown;
        private bool _isSpawningActive = false;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void OnEnable() {
            EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void Start() {
            poolSystemService = ServiceRegistry.Get<IPoolSystem>();
            gameSystemService = ServiceRegistry.Get<IGameSystem>();

            // Initialize first spawn cooldown
            _spawnCooldown = gameSystemService.LevelData.GetSpawnInterval(0f);
        }

        private void Update() {
            InitializeSpawning();
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            if (evt.NewState == GameState.Playing) {
                _isSpawningActive = true;
            } else {
                _isSpawningActive = false;
            }
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Updates the spawn cooldown.
        /// </summary>
        private void InitializeSpawning() {
            if (!_isSpawningActive) return;

            _spawnCooldown -= Time.deltaTime;

            if (_spawnCooldown <= 0f) {
                Spawn();
                // Fetch next cooldown based on the updated time
                _spawnCooldown = gameSystemService.LevelData.GetSpawnInterval(gameSystemService.ElapsedTime);
            }
        }

        /// <summary>
        /// Spawns a random item at a random spawn point.
        /// </summary>
        private void Spawn() {
            if (itemData == null || itemData.Length == 0) return;
            if (spawnPoints == null || spawnPoints.Length == 0) return;

            ItemDataSO selectedItem = GetRandomItemByWeight();
            if (selectedItem?.PoolItemSO == null) return;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Retrieve from your pool service at target position/rotation
            GameObject itemInstance = poolSystemService.SpawnFromPool(selectedItem.PoolItemSO.name, spawnPoint.position, spawnPoint.rotation);

            if (itemInstance != null) {
                if (itemInstance.TryGetComponent<ItemController>(out ItemController itemController)) {
                    itemController.SetSpawner(this);
                } 
            }
        }

        /// <summary>
        /// Despawns an item and returns it to the pool.
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="itemInstance"></param>
        private void Despawn(ItemDataSO itemData, GameObject itemInstance) {
            poolSystemService.ReturnToPool(itemData.PoolItemSO.name, itemInstance);
        }

        /// <summary>
        /// Selects an item using standard roulette-wheel weighted probability.
        /// </summary>
        private ItemDataSO GetRandomItemByWeight() {
            float totalWeight = 0f;
            for (int i = 0; i < itemData.Length; i++) {
                totalWeight += itemData[i].Probability;
            }

            if (totalWeight <= 0f) {
                return itemData[Random.Range(0, itemData.Length)];
            }

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < itemData.Length; i++) {
                cumulative += itemData[i].Probability;
                if (roll <= cumulative) {
                    return itemData[i];
                }
            }

            return itemData[itemData.Length - 1];
        }

        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        /// <summary>
        /// Despawns an item and returns it to the pool.
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="itemInstance"></param>
        public void ItemDespawnQueue(ItemDataSO itemData, GameObject itemInstance) => Despawn(itemData, itemInstance);
    }
}