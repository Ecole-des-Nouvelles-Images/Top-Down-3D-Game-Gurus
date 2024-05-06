using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class TurtlePlayerController : MonoBehaviour
    {
        [Header("STATS")] public float moveSpeed = 3f;
        public float attackDistance = 2f;
        public float attackWidth = 1f;

        public float dashSpeedMultiplier = 2f;
        public float maxDashChargeTime = 3f;
        public float minDashDistance = 3f;
        public float maxDashDistance = 10f;

        [Header("VARIABLES")] public Rigidbody rb;
        public GameObject TrapPrefab;
        public ParticleSystem attackParticle;
        public ParticleSystem dashParticle;

        private Vector2 move;
        private bool isDashing = false;
        private bool isCharging = false;
        private float dashChargeStartTime;
        private Collider[] cachedColliders;

        private void FixedUpdate()
        {
            if (!isCharging || !isDashing)
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
            if (!isCharging)
            {
                move = context.ReadValue<Vector2>();
            }
            else
            {
                Vector2 dashDirection = context.ReadValue<Vector2>();
                Vector3 movement = new Vector3(dashDirection.x, 0f, dashDirection.y) * moveSpeed;
                Quaternion newRotation = Quaternion.LookRotation(-movement, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, 0.15f);
            }
        }

        private void MovePlayer()
        {
            if (!isCharging)
            {
                Vector3 movement = new Vector3(-move.x, 0f, -move.y) * moveSpeed;

                if (movement != Vector3.zero && !isCharging)
                {
                    Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                    rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, 0.15f);
                }

                rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            }
            else
            {
                rb.velocity = Vector3.zero;
        
                if (!isDashing)
                {
                    move = Vector2.zero;
                }
            }
        }



        #endregion


        #region Dash

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isCharging = true;
                dashChargeStartTime = Time.time;
                Debug.Log("Début de la charge du dash.");
            }
            else if (context.canceled && !isDashing)
            {
                isCharging = false;
                float dashChargeTime = Time.time - dashChargeStartTime;

                float dashDistance = Mathf.Lerp(minDashDistance, maxDashDistance,
                    Mathf.Clamp01(dashChargeTime / maxDashChargeTime));
                float dashSpeed = dashSpeedMultiplier * dashDistance / maxDashDistance;

                Dash(dashDistance, dashSpeed); // Appeler la méthode Dash() lorsque le dash est annulé
                Debug.Log("Dash annulé.");
            }
        }


        
        private void Dash(float distance, float speed)
        {
            //    dashParticle.Play();
            
            Vector3 dashDirection = transform.forward;
            rb.velocity = dashDirection * speed;
            isDashing = true;
        }

        #endregion

        #region Attack

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //     attackParticle.Play();
                Attack();
            }
        }

        private void Attack()
        {
            if (cachedColliders == null)
            {
                Vector3 attackOrigin = transform.position + transform.forward * attackDistance;
                cachedColliders = Physics.OverlapCapsule(attackOrigin, attackOrigin + transform.forward * attackWidth,
                    attackWidth / 2f);
            }
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
            Gizmos.DrawLine(attackOrigin + Vector3.left * attackWidth / 2f,
                attackOrigin + transform.forward * attackWidth / 2f);
            Gizmos.DrawLine(attackOrigin + Vector3.right * attackWidth / 2f,
                attackOrigin + transform.forward * attackWidth / 2f);
        }

        #endregion
    }
}