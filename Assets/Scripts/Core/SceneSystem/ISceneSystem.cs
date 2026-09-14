using UnityEngine;
using Core.Enums.SceneSystem;

namespace Core.ServiceLocator {
    public interface ISceneSystem {
        public void LoadScene(SceneID targetSceneID, bool showLoadingScreen = false, bool isAdditive = false, bool isAsync = false, bool isLoadFirst = false);

    }
}