using UnityEngine;
using Features.ItemSpawner;

namespace Features.Item {
    public class ItemController : MonoBehaviour {
        [Header("Item Downward Speed")]
        [SerializeField] private float downwardSpeed = 5f;

        [Header("ItemData")]
        [SerializeField] private ItemDataSO itemData;

        // --- Private Properties ---
        private Rigidbody2D rb;
        private Collider2D col;
        
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

        private void FixedUpdate() {
            rb.linearVelocity = Vector2.down * downwardSpeed;
        }
    }
}