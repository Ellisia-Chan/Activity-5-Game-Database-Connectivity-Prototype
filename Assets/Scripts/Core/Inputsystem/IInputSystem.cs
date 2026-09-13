using UnityEngine;

namespace Core.ServiceLocator {
    /// <summary>
    /// Service interface for InputSystem
    /// </summary>
	public interface IInputSystem {

        /// <summary>
        /// Returns the movement input vector
        /// </summary>
        /// <returns>The movement input vector</returns>
        public Vector2 GetMovementVector();
    }
}