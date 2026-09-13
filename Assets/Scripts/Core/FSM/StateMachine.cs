using UnityEngine;

namespace Core.FSM {
    /// <summary>
    /// FSM
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    public class StateMachine<TOwner> {
        public  IState currentState { get; private set; }
        private TOwner owner;

        public StateMachine(TOwner owner) {
            this.owner = owner;
        }

        /// <summary>
        /// Change state
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState newState) {
            if (currentState == newState) return;

            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        /// <summary>
        /// Update state
        /// </summary>
        public void Update() {
            currentState?.OnUpdate();
        }


        /// <summary>
        /// Fixed update
        /// </summary>
        public void FixedUpdate() {
            currentState?.OnFixedUpdate();
        }
    }
}