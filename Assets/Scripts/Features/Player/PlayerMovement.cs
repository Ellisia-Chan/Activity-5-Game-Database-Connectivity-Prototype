using UnityEngine;

using ServiceLocator;
using Services.InputSystem;

namespace Features.Player {

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour {

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 6f;


        private Rigidbody2D rb;

        // --- Services ---
        private IInputSystem inputSystem;


        private Vector2 moveDir;

        // --- Public Properties ---
        public bool isMoving { get; private set; }
        public float horizontalDir { get; private set; }

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Start() {
            inputSystem = ServiceRegistry.Get<IInputSystem>();

            isMoving = false;
        }

        private void Update() {
            HandleInput();
        }

        private void FixedUpdate() {
            HandleMovement();
            SpeedControl();
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Handles player input
        /// </summary>
        /// <returns></returns>
        private void HandleInput() {
            moveDir = inputSystem.GetMovementVector();

            // Snaps horizontal input directly to -1, 0, or 1
            if (Mathf.Abs(moveDir.x) > 0.01f) {
                horizontalDir = Mathf.Sign(moveDir.x);
            }
            else {
                horizontalDir = 0f;
            }

            isMoving = moveDir.sqrMagnitude > 0f;
        }


        /// <summary>
        /// Handles player movement
        /// </summary>
        /// <returns></returns>
        private void HandleMovement() {
            rb.linearVelocity = new Vector2(horizontalDir * moveSpeed, rb.linearVelocity.y);
        }

        /// <summary> 
        /// Controls player speed 
        /// </summary>
        /// <returns></returns>
        private void SpeedControl() {
            if (rb.linearVelocity.sqrMagnitude > moveSpeed * moveSpeed) {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
    }
}
