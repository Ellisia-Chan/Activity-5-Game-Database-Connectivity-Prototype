using UnityEngine;
using Core.ServiceLocator;
using Core.FSM;
using Core.Enums;
using Core.EventSystem;
using Core.Events.GameSystem;

namespace Core.GameSystem {
    /// <summary>
    /// The game manager. Handles the game lifecycle
    /// </summary>
    public class GameManager : MonoBehaviour, IGameSystem {

        [Header("Starting State")]
        [Tooltip("The starting state of the game")]
        [SerializeField] private GameState startingState = GameState.Waiting;

        [Header("Level Data")]
        [SerializeField] private LevelData levelData;

        // --- Private Properties ---
        private StateMachine<GameManager> gameFSM;

        private GameState lastGameState;

        // --- Public Properties ---
        public GameState CurrentGameState { get; private set; }


        // --- Interface Properties
        public float ElapsedTime { get; private set; }
        public LevelData LevelData => levelData;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (ServiceRegistry.IsRegistered<IGameSystem>()) {
                Destroy(gameObject);
                return;
            }
            else {
                ServiceRegistry.Register<IGameSystem>(this);
            }
        }

        private void Start() {
            gameFSM = new StateMachine<GameManager>(this);

            SetGameState(startingState);
        }

        private void Update() {
            gameFSM?.Update();
            HandleGameTimer();
        }

        private void FixedUpdate() {
            gameFSM?.FixedUpdate();
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<IGameSystem>(this);
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        private void HandleGameTimer() {
            if (CurrentGameState == GameState.Playing) {
                ElapsedTime += Time.deltaTime;
            }

            if (ElapsedTime >= levelData.levelDuration) {
                SetGameState(GameState.Over);
            }
        }

        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        /// <summary>
        /// Sets the game state
        /// </summary>
        /// <param name="state"></param>
        public void SetGameState(GameState state) {
            CurrentGameState = state;

            switch (state) {
                case GameState.Waiting:
                    gameFSM.ChangeState(new GameState_Waiting(this, gameFSM));
                    break;
                case GameState.Playing:
                    gameFSM.ChangeState(new GameState_Playing(this, gameFSM));
                    break;
                case GameState.Paused:
                    gameFSM.ChangeState(new GameState_Paused(this, gameFSM));
                    break;
                case GameState.Over:
                    gameFSM.ChangeState(new GameState_Over(this, gameFSM));
                    break;
                default:
                    break;
            }

            EventBus.Publish(new Evt_OnGameStateChanged(state));
        }
    }
}