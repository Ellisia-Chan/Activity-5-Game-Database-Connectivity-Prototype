namespace FSM {
    public abstract class State<TOwner> : IState {
        protected TOwner owner;
        protected StateMachine<TOwner> stateMachine;

        public State(TOwner owner, StateMachine<TOwner> stateMachine) {
            this.owner = owner;
            this.stateMachine = stateMachine;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void OnFixedUpdate();
    }

}