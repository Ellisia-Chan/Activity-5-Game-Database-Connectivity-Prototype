using System.Collections.Generic;
using UnityEngine;

using SO;

using ServiceLocator;
using ServiceLocator.Services;

namespace PoolSystem {
    public class PoolRuntimeSystem : MonoBehaviour, IPoolSystem {

        [SerializeField] private List<PoolItemSO> pools;
        private Dictionary<string, Queue<GameObject>> poolDictionary;
        private Dictionary<string, PoolItemSO> poolItemDictionary;
        private Dictionary<string, Transform> poolParents;

        private void Awake() {
            if (ServiceRegistry.IsRegistered<IPoolSystem>()) {
                Debug.LogWarning("[PoolRuntimeSystem] IPoolSystem is already registered");
                Destroy(gameObject);
                return;
            }

            ServiceRegistry.Register<IPoolSystem>(this);

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            poolItemDictionary = new Dictionary<string, PoolItemSO>();
            poolParents = new Dictionary<string, Transform>();

            foreach (PoolItemSO pool in pools) {
                string parentName = "Pool_" + pool.itemName;
                GameObject parentObject = new GameObject(parentName);
                parentObject.transform.SetParent(transform);

                poolParents[pool.itemName] = parentObject.transform;

                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.poolSize; i++) {
                    GameObject obj = Instantiate(pool.prefab, pool.resetPosition, Quaternion.identity, parentObject.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.itemName, objectPool);
                poolItemDictionary.Add(pool.itemName, pool);
            }
        }

        private void OnDestroy() {
            if (ServiceRegistry.IsRegistered<IPoolSystem>()) {
                ServiceRegistry.Unregister<IPoolSystem>(this);
            }
        }



        public GameObject SpawnFromPool(string itemName, Vector3 position, Quaternion rotation = default, Transform parent = null) {
            if (!poolDictionary.TryGetValue(itemName, out Queue<GameObject> poolQueue)) {
                Debug.LogWarning("Pool with name " + itemName + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn;
            
            // Check if pool is empty to avoid InvalidOperationException and dynamically expand if necessary
            if (poolQueue.Count > 0) {
                objectToSpawn = poolQueue.Dequeue();
            } else {
                Debug.LogWarning($"[PoolSystem] Pool '{itemName}' is empty. Instantiating a new object. Consider increasing initial poolSize to avoid frame spikes.");
                PoolItemSO poolItem = poolItemDictionary[itemName];
                Transform cachedParent = poolParents[itemName];
                objectToSpawn = Instantiate(poolItem.prefab, cachedParent);
                objectToSpawn.SetActive(false);
            }

            // Always reparent BEFORE activating the object to avoid triggering expensive hierarchy/physics updates
            if (parent != null) {
                objectToSpawn.transform.SetParent(parent, false);
            } else if (objectToSpawn.transform.parent != poolParents[itemName]) {
                // Ensure it's back to its pool root if no parent is provided
                objectToSpawn.transform.SetParent(poolParents[itemName], false);
            }

            // Use SetPositionAndRotation: Updates position and rotation in a single native call, 
            // which is significantly faster than assigning them separately.
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);
            
            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        public void ReturnToPool(string itemName, GameObject objectToReturn, Transform position = null, Quaternion rotation = default, Transform parent = null) {
            if (!poolDictionary.TryGetValue(itemName, out Queue<GameObject> poolQueue)) {
                Debug.LogWarning("Pool with name " + itemName + " doesn't exist.");
                return;
            }

            // SetActive FALSE immediately to prevent physics/render updates during modifications
            objectToReturn.SetActive(false);

            // Note: We don't reset the position/rotation here. It wastes CPU time to move an inactive object.
            // The position is guaranteed to be overwritten during the next SpawnFromPool call.

            if (parent != null) {
                if (objectToReturn.transform.parent != parent) {
                    objectToReturn.transform.SetParent(parent, false);
                }
            }
            else if (poolParents.TryGetValue(itemName, out Transform cachedParent)) {
                if (objectToReturn.transform.parent != cachedParent) {
                    objectToReturn.transform.SetParent(cachedParent, false);
                }
            }
            else {
                Debug.LogWarning($"No parent found for pool '{itemName}'");
            }

            poolQueue.Enqueue(objectToReturn);
        }
    }
}