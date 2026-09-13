using Core.Enums.GameSystem;
using Core.Events.GameSystem;
using Core.Events.InputSystem;
using Core.Events.UI;
using Core.EventSystem;
using Core.FSM;
using Core.ServiceLocator;
using Editor;
using UnityEngine;

namespace Features.GameSystem {
    /// <summary>
    /// The game manager. Handles the game lifecycle
    /// </summary>
    public class GameManager : MonoBehaviour, IGameSystem {

        [Header("Starting State")]
        [Tooltip("The starting state of the game")]
        [SingleSelectionFlag]
        [SerializeField] private GameState startingState = GameState.Waiting;

        [Header("Level Data")]
        [SerializeField] private LevelData levelData;

        // --- Private Properties ---
        private StateMachine<GameManager> gameFSM;


        // --- Public Properties ---
        public GameState CurrentGameState { get; private set; }
        public GameState LastGameState { get; private set; }


        // --- Interface Properties ---
        public float RemainingTime { get; private set; }
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

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnPauseAction>(OnPauseAction);
            EventBus.Subscribe<Evt_OnResumeButtonAction>(OnResumeButtonAction);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnPauseAction>(OnPauseAction);
            EventBus.Unsubscribe<Evt_OnResumeButtonAction>(OnResumeButtonAction);
        }

        private void Start() {
            gameFSM = new StateMachine<GameManager>(this);

            SetGameState(startingState);

            RemainingTime = levelData.levelDuration;
        }

        private void Update() {
            gameFSM?.Update();
        }

        private void FixedUpdate() {
            gameFSM?.FixedUpdate();
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<IGameSystem>(this);
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnPauseAction(Evt_OnPauseAction evt) {
            if (CurrentGameState == GameState.Playing) {
                SetGameState(GameState.Paused);
            }
            else if (CurrentGameState == GameState.Paused) {
                SetGameState(GameState.Playing);
            }
        }

        private void OnResumeButtonAction(Evt_OnResumeButtonAction evt) {
            SetGameState(GameState.Playing);
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
            if (CurrentGameState == state) return;

            CurrentGameState = state;
            LastGameState = state;

            switch (CurrentGameState) {
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

            EventBus.Publish(new Evt_OnGameStateChanged(CurrentGameState));
        }

        /// <summary>
        /// Updates the remaining game time
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateRemainingTime(float deltaTime) {
            RemainingTime -= deltaTime;
        }
    }
}