using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f;
        private bool isDoingMainCapacity = false;

        protected Rigidbody Rb;
        private Vector2 move;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (!isDoingMainCapacity)
            {
                Move();
            }
        }
        
        private void Update()
        {
            if (isDoingMainCapacity && Rb.velocity.magnitude < 0.01f)
            {
                isDoingMainCapacity = false; 
            }
        }


        #region Move
        
        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        private void Move()
        {
            Vector3 movement = new Vector3(-move.x, 0f, -move.y) * moveSpeed;

            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                Rb.rotation = Quaternion.Slerp(Rb.rotation, newRotation, 0.15f);
            }

            Rb.velocity = new Vector3(movement.x, Rb.velocity.y, movement.z);
        }
        #endregion
        
        #region Main Capacity
        
        public void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.performed && !isDoingMainCapacity) 
            {
                MainCapacity();
                isDoingMainCapacity = true;
            }
        }
        
        protected abstract void MainCapacity();
        #endregion
    }
}