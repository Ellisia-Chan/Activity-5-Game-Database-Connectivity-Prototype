using Core.Enums.SceneSystem;
using Core.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneSystem {
    public class SceneManager : MonoBehaviour, ISceneSystem {
        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (ServiceRegistry.IsRegistered<ISceneSystem>()) {
                Destroy(gameObject);
                return;
            }
            else {
                ServiceRegistry.Register<ISceneSystem>(this);
            }
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<ISceneSystem>(this);
        }


        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        public void LoadScene(SceneID targetSceneID, bool showLoadingScreen = false, bool isAdditive = false, bool isAsync = false, bool isLoadFirst = false) {

        }
    }
}