using UnityEngine;
using Features.ItemSpawnerSystem;
using Core.EventSystem;
using Core.Events.GameSystem;
using Core.Enums;

namespace Features.Item {
    /// <summary>
    ///  Controls the item's movement and collected logic.
    /// </summary>
    public class ItemController : MonoBehaviour, ICollectible {
        [Header("Item Downward Speed")]
        [SerializeField] private float downwardSpeed = 5f;

        [Header("ItemData")]
        [SerializeField] private ItemDataSO itemData;

        // --- Private Properties ---
        private Rigidbody2D rb;
        private Collider2D col;
        private ItemSpawner itemSpawner;
        
        // --- Public Properties ---
        public ItemDataSO ItemData => itemData;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void OnEnable() {
           EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            ResetState();

            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void FixedUpdate() {
            rb.linearVelocity = Vector2.down * downwardSpeed;
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            if (evt.NewState == GameState.Over) {
                ReturnToSpawner();
            }
        }


        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        private void ResetState() {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        // =====================================================================
        //
        //                          Public Methods
        //
        // =====================================================================
        public void SetSpawner(ItemSpawner itemSpawner) => this.itemSpawner = itemSpawner;

        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================
        public void ReturnToSpawner() {
            itemSpawner.ItemDespawnQueue(itemData, gameObject);
        }
    }
}