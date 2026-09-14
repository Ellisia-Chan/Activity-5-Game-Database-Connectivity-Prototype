using UnityEngine;
using UnityEngine.InputSystem;

using Core.ServiceLocator;
using Core.EventSystem;
using Core.Events.InputSystem;

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

            inputActions.Player.Skill.performed += OnSkillPerformed;

            inputActions.General.Pause.performed += OnPauseAction;
        }

        private void OnDisable() {
            inputActions.Disable();

            inputActions.Player.Skill.performed -= OnSkillPerformed;

            inputActions.General.Pause.performed -= OnPauseAction;
        }

        private void OnDestroy() {
            ServiceRegistry.Unregister<IInputSystem>(this);
        }

        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        private void OnSkillPerformed(InputAction.CallbackContext context) {
            EventBus.Publish(new Evt_OnSkillPerformed());
        }

        private void OnPauseAction(InputAction.CallbackContext context) {
            EventBus.Publish(new Evt_OnPauseAction());
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