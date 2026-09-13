using UnityEngine;

using Core.ServiceLocator;

namespace InputSystem {
    /// <summary>
    /// Manages the input system for the game
    /// </summary>
    public class InputManager : MonoBehaviour, IInputSystem {

        private InputSystem_Actions inputActions;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            inputActions = new InputSystem_Actions();

            if (ServiceRegistry.IsRegistered<IInputSystem>()) {
                Destroy(gameObject);
                return;
            } else {
                ServiceRegistry.Register<IInputSystem>(this);
            }
        }

        private void OnEnable() {
            inputActions.Enable();
        }

        private void OnDisable() {
            inputActions.Disable();
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<IInputSystem>(this);
        }
        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        /// <summary>
        /// Returns the movement input vector of the player
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMovementVector() {
            return inputActions.Player.Move.ReadValue<Vector2>().normalized;
        }
    }
}