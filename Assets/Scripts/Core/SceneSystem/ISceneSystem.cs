using UnityEngine;
using Core.Enums.SceneSystem;

namespace Core.ServiceLocator {
    /// <summary>
    /// Interface Service for SceneSystem
    /// </summary>
    public interface ISceneSystem {
        public void LoadScene(
            SceneID targetSceneID,
            SceneID loadingScreenID = SceneID.None,
            bool showLoadingScreen = false,
            bool isAdditive = false,
            bool isAsync = false,
            bool unloadCurrentFirst = false,
            SceneID sceneToUnload = SceneID.None
            );
    }
}