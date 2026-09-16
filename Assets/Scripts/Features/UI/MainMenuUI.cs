using UnityEngine;
using Core.ServiceLocator;
using Core.Enums.SceneSystem;

namespace Features.UI {
    public class MainMenuUI : MonoBehaviour {
        [Header("UI Elements")]
        [SerializeField] private GameObject mainMenuPanel;

        // --- Service Dependencies ---
        private ISceneSystem sceneSystemService;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            sceneSystemService = ServiceRegistry.Get<ISceneSystem>();
        }

        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        /// <summary>
        /// Starts the game
        /// </summary>
        public void PlayButton() {
            sceneSystemService.LoadScene(SceneID.GameScene, SceneID.LoadingScene, showLoadingScreen: true, isAsync: true);
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void QuitButton() {
            Application.Quit();
        }
    }
}