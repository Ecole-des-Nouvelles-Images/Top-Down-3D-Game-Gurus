using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Noah.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f;

        protected Rigidbody Rb;
        protected Vector2 move;
        private CinemachineTargetGroup _targetGroup;
        private Transform _transform;

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
            Vector3 movement = new Vector3(move.x, 0f, move.y) * moveSpeed;

            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                Rb.rotation = Quaternion.Slerp(Rb.rotation, newRotation, 0.15f);

            }
            Vector3 normalizedMovement = movement.normalized;
            Rb.AddForce(normalizedMovement * Time.deltaTime, ForceMode.Force);
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

        #region Third Capacity

        public virtual void OnThirdCapacity(InputAction.CallbackContext context)
        {
            ThirdCapacity();
        }

        protected virtual void ThirdCapacity()
        {
            Debug.Log("ThirdCapacity is not used");
        }

        #endregion

        private void OnDrawGizmos()
        {
            Vector3 forward = transform.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, forward * 2f);
        }
    }
}
