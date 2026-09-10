namespace Core.FSM {
    /// <summary>
    /// Base class for all states in the state machine
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    public abstract class State<TOwner> : IState {
        protected TOwner owner;
        protected StateMachine<TOwner> stateMachine;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="stateMachine"></param>
        public State(TOwner owner, StateMachine<TOwner> stateMachine) {
            this.owner = owner;
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// Called when the state is entered
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Called when the state is updated
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// Called when the state is exited
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Called when the state is fixed updated
        /// </summary>
        public abstract void OnFixedUpdate();
    }

}