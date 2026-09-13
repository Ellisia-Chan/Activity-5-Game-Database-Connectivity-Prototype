using UnityEngine;

namespace Core.FSM {
    /// <summary>
    /// Interface for states of Finite State Machine
    /// </summary>
	public interface IState {

        /// <summary>
        /// Enter state
        /// </summary>
		void OnEnter();

        /// <summary>
        /// Update state
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// Exit state
        /// </summary>
        void OnExit();

        /// <summary>
        /// Fixed update
        /// </summary>
		void OnFixedUpdate();
    }
}