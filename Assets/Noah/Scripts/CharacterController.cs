using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f;

        protected Rigidbody Rb;
        protected Vector2 move;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Update() { }

        #region Move

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        protected void Move()
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

        public virtual void OnMainCapacity(InputAction.CallbackContext context)
        {
            MainCapacity();
        }

        protected abstract void MainCapacity();

        #endregion

        
        #region Secondary Capacity

        public virtual void OnSecondaryCapacity(InputAction.CallbackContext context)
        {
            SecondaryCapacity();
        }

        protected abstract void SecondaryCapacity();

        #endregion
        
    }
}