using UnityEngine;

namespace Core.ServiceLocator {
    /// <summary>
    /// Interface for the IPoolSystem service.
    /// </summary>
    public interface IPoolSystem {
        /// <summary>
        /// Spawns an object from the specified pool.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns>Specified <see cref="GameObject"/></returns>
        public GameObject SpawnFromPool(
            string itemName,
            Vector3 position,
            Quaternion rotation = default,
            Transform parent = null
        );

        /// <summary>
        /// Returns an object to the specified pool.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="objectToReturn"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        public void ReturnToPool(
            string itemName,
            GameObject objectToReturn,
            Transform position = null,
            Quaternion rotation = default,
            Transform parent = null
        );
    }
}