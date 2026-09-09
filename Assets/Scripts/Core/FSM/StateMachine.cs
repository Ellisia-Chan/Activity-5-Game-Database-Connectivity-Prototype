using UnityEngine;

namespace FSM {
    public class StateMachine<TOwner> {
        public  IState currentState { get; private set; }
        private TOwner owner;

        public StateMachine(TOwner owner) {
            this.owner = owner;
        }

        public void ChangeState(IState newState) {
            if (currentState == newState) return;

            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void Update() {
            currentState?.OnUpdate();
        }

        public void FixedUpdate() {
            currentState?.OnFixedUpdate();
        }
    }
}