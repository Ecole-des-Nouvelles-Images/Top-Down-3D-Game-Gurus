using Michael.Scripts.Manager;
using Michael.Scripts.Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts.Controller
{
    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed ;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected float idleTreshold = 0.1f;
        public Rigidbody Rb;
        protected Vector2 move;
        private static readonly int Run = Animator.StringToHash("Run");

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        public void OnPause()
        {
            if (!PauseControlller.IsPaused)
            {
                PauseControlller.Instance.OpenPausePanel();
            }
          
        }
        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Update()
        {
            
        }

        #region Move

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!PauseControlller.IsPaused)
            {
                move = context.ReadValue<Vector2>();
            }
            
        }

        protected virtual void Move()
        { 
            Vector3 movement = new Vector3(move.x, 0f, move.y) * moveSpeed;
            if ( movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                Rb.rotation = Quaternion.Slerp(Rb.rotation, newRotation, 0.15f);
            }
            
            // Rb.MovePosition(transform.position + new Vector3(movement.x, 2, movement.z) * Time.deltaTime);
            Rb.AddForce(movement * Time.deltaTime, ForceMode.Force);
            // Rb.velocity = new Vector3(movement.x, Rb.velocity.y, movement.z);
            
        }

        #endregion

        #region Main Capacity

        public virtual void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.performed && !PauseControlller.IsPaused)
            {
                MainCapacity();
            }
        }

        protected abstract void MainCapacity();

        #endregion
        
        #region Secondary Capacity

        public virtual void OnSecondaryCapacity(InputAction.CallbackContext context)
        {
            if (context.performed && !PauseControlller.IsPaused)
            {
                SecondaryCapacity();
            }
           
        }

        protected abstract void SecondaryCapacity();

        #endregion
        
        #region Third Capacity

        public virtual void OnThirdCapacity(InputAction.CallbackContext context)
        {
            if (context.performed  && !PauseControlller.IsPaused)
            {
                ThirdCapacity();
            }
           
        }

        protected virtual void ThirdCapacity()
        {
          
        }

        #endregion
        
        
        #region Fourth Capacity
        public virtual void OnFourthCapacity(InputAction.CallbackContext context)
        {
            if (context.performed  && !PauseControlller.IsPaused)
            {
               FourthCapacity();
            }
           
        }

        protected virtual void FourthCapacity()
        {
            
        }
        #endregion

        
        
        
    }
}