using UnityEngine;

namespace Features.Player {
	public class PlayerAnimations : MonoBehaviour {

		[Header("Components")] 
        [SerializeField] private Animator _animator;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private string isWalkingAnimationParam = "isWalking";

        [Header("Dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            if (_animator == null || _spriteRenderer == null) { Debug.LogError("Missing components"); }
        }

        private void Update() {
            HandleAnimations();
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
            if (_playerMovement.isMoving) {
                _animator.SetBool(isWalkingAnimationParam, true);
                _spriteRenderer.flipX = _playerMovement.horizontalDir < 0f;
            }
            else {
                _animator.SetBool(isWalkingAnimationParam, false);
            }
        }
    }
}