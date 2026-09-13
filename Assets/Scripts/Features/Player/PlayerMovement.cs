using UnityEngine;

using Core.ServiceLocator;
using Core.EventSystem;
using Core.Events.GameSystem;
using Core.Enums;

namespace Features.Player {
    /// <summary>
    /// Handles player movement 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour {

        [Header("Movement Settings")]
        [SerializeField, Min(0f)] private float moveSpeed = 6f;

        // --- Service Dependencies ---
        private IInputSystem inputSystemService;

        // --- Private properties ---
        private Rigidbody2D rb;
        private Vector2 moveDir;
        private GameState currentGameState;

        // --- Public Properties ---
        public bool IsMoving { get; private set; }
        public float HorizontalDir { get; private set; }

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

        private void OnEnable() {
            EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void Start() {
            inputSystemService = ServiceRegistry.Get<IInputSystem>();

            IsMoving = false;
        }

        private void Update() {
            HandleInput();
        }

        private void FixedUpdate() {
            if (currentGameState == GameState.Playing) {
                HandleMovement();
                SpeedControl(); 
            }
        }

        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            currentGameState = evt.NewState;
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
            moveDir = inputSystemService.GetMovementVector();

            if (Mathf.Abs(moveDir.x) > 0.01f) {
                HorizontalDir = Mathf.Sign(moveDir.x);
            }
            else {
                HorizontalDir = 0f;
            }

            IsMoving = moveDir.sqrMagnitude > 0f;
        }


        /// <summary>
        /// Handles player movement
        /// </summary>
        /// <returns></returns>
        private void HandleMovement() {
            rb.linearVelocity = new Vector2(HorizontalDir * moveSpeed, rb.linearVelocity.y);
        }

        /// <summary> 
        /// Controls player speed to max specified speed
        /// </summary>
        /// <returns></returns>
        private void SpeedControl() {
            if (rb.linearVelocity.sqrMagnitude > moveSpeed * moveSpeed) {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
    }
}
