using UnityEngine;
using Core.FSM;
using Core.Enums;

namespace Core.GameSystem {
    /// <summary>
    /// The waiting state of the game
    /// </summary>
    public class GameState_Waiting : State<GameManager> {
        // Constructor
        public GameState_Waiting(GameManager owner, StateMachine<GameManager> stateMachine) : base(owner, stateMachine) { }


        public override void OnEnter() {
            owner.SetGameState(GameState.Playing);
        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {

        }
    }

    /// <summary>
    /// The playing state of the game
    /// </summary>
    public class GameState_Playing : State<GameManager> {
        // Constructor
        public GameState_Playing(GameManager owner, StateMachine<GameManager> stateMachine) : base(owner, stateMachine) { }

        public override void OnEnter() {

        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {

        }
    }

    /// <summary>
    /// The paused state of the game
    /// </summary>
    public class GameState_Paused : State<GameManager> {
        // Constructor
        public GameState_Paused(GameManager owner, StateMachine<GameManager> stateMachine) : base(owner, stateMachine) { }

        public override void OnEnter() {

        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {

        }
    }

    /// <summary>
    /// The over state of the game
    /// </summary>
    public class GameState_Over : State<GameManager> {
        // Constructor
        public GameState_Over(GameManager owner, StateMachine<GameManager> stateMachine) : base(owner, stateMachine) { }

        public override void OnEnter() {

        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {

        }
    }
}