using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts
{
     public class PlayerController : MonoBehaviour
    {
        [Header("VARIABLES")] 
        public Rigidbody rb;
        public GameObject TrapPrefab;

        [Header("STATS")] 
        public float moveSpeed = 3f;
        public float dashSpeed = 2f;
        public float attackDistance = 2f; 
        public float attackWidth = 1f; 

        private Vector2 move;
        private bool isDashing = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() 
        {
            if (!isDashing) 
            {
                MovePlayer();
            }
        }
        
        private void Update()
        {
            if (isDashing && rb.velocity.magnitude < 0.1f)
            {
                isDashing = false; 
            }
        }

        #region Move
        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        private void MovePlayer()
        {
            Vector3 movement = new Vector3(move.x, 0f, move.y) * moveSpeed;

            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, 0.15f);
            }

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        #endregion
        
        #region Dash
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && !isDashing) 
            {
                Dash();
            }
        }
        
        private void Dash()
        {
            Vector3 dashDirection = transform.forward;
            rb.velocity = dashDirection * dashSpeed;
            isDashing = true;
        }
        #endregion

        #region Attack
        public void OnAttack(InputAction.CallbackContext context)
        {
         
        }
        
       

        #endregion
        
        #region Trap
        public void OnTrap(InputAction.CallbackContext context)
        {
            SetTrap();
        }
        
        private void SetTrap()
        {
            Instantiate(TrapPrefab, transform.position, transform.rotation);
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos()
        {
            Vector3 attackOrigin = transform.position + transform.forward * attackDistance;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin, attackWidth / 2f);
            Gizmos.DrawLine(attackOrigin + Vector3.left * attackWidth / 2f, attackOrigin + transform.forward * attackWidth / 2f);
            Gizmos.DrawLine(attackOrigin + Vector3.right * attackWidth / 2f, attackOrigin + transform.forward * attackWidth / 2f);
        }
        #endregion
    }
}
