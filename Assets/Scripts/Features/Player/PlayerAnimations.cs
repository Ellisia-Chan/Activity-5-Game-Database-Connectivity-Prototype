using UnityEngine;
using Core.Enums.GameSystem;
using Core.EventSystem;
using Core.Events.GameSystem;

namespace Features.Player {
    /// <summary>
    /// Handles player animations 
    /// </summary>
	public class PlayerAnimations : MonoBehaviour {

		[Header("Components")] 
        [SerializeField] private Animator _animator;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private string isWalkingAnimationParam = "isWalking";

        [Header("Dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;

        // --- Private Properties ---
        private GameState currenGameState;
        private bool resetParam = false;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (_animator == null || _spriteRenderer == null) { Debug.LogError("Missing components"); }
        }

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void Update() {
            if (currenGameState == GameState.Playing) {
                HandleAnimations(); 
            } else if (resetParam) {
                ResetAnimation(false);
            }
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            currenGameState = evt.NewState;

            if (currenGameState != GameState.Playing) { resetParam = true; }
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Handles player animations
        /// </summary>
        /// <returns></returns>
        private void HandleAnimations() {
            if (_playerMovement.IsMoving) {
                _animator.SetBool(isWalkingAnimationParam, true);
                _spriteRenderer.flipX = _playerMovement.HorizontalDir < 0f;
            }
            else {
                _animator.SetBool(isWalkingAnimationParam, false);
            }
        }

        /// <summary>
        /// Resets player animations to idle
        /// </summary>
        /// <param name="reset"></param>
        private void ResetAnimation(bool reset) {
            _animator.SetBool(isWalkingAnimationParam, reset);

            resetParam = false;
        }
    }
}