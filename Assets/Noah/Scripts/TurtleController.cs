using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class TurtleController : CharacterController
    {
        [Header("Dashing")] [SerializeField] private float dashForce;
        [SerializeField] private float maxDashForce;
        [SerializeField] private bool isDashing;
        [SerializeField] private bool isCharging;
        [SerializeField] private bool isDashInputReleased;
        [SerializeField] private float chargeDashMultiplier;
        [SerializeField] private float chargeTime;
        [SerializeField] private float maxChargeTime;

        [SerializeField]private Collider _attackCollider;
        private Vector2 lastNonZeroDirection;

        
        private void Start()
        {
            _attackCollider.enabled = false;
        }
        
        protected override void FixedUpdate()
        {
            if (!isDashing && !isCharging)
            {
                Move();
            }
        }

        protected override void Update()
        {
            DashingUpdate();
        }

        #region Main Capacity

        public override void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                StartCharging();
            }
            else if (context.canceled)
            {
                StopCharging();
                MainCapacity();
            }
        }

        protected override void MainCapacity()
        {
            Debug.Log("turtle main capacity");

            if (!isDashing)
            {
                Vector2 dashDirection;

                if (move.magnitude == 0f)
                {
                    dashDirection = isDashInputReleased
                        ? -lastNonZeroDirection
                        : new Vector2(transform.forward.x, transform.forward.z);
                }
                else
                {
                    dashDirection = isDashInputReleased ? -move.normalized : move.normalized;
                }

                float currentDashForce = Mathf.Clamp(dashForce * (chargeTime * chargeDashMultiplier), 0f, maxDashForce);
                Rb.velocity = new Vector3(dashDirection.x, 0f, dashDirection.y) * currentDashForce;
                isDashing = true;
            }
        }

        private void DashingUpdate()
        {
            if (isDashing && Rb.velocity.magnitude < 0.01f)
            {
                isDashing = false;
            }

            if (isCharging)
            {
                chargeTime += Time.deltaTime;
                chargeTime = Mathf.Min(chargeTime, maxChargeTime);

                if (move.magnitude > 0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(new Vector3(-move.x, 0f, -move.y), Vector3.up);
                    Rb.rotation = Quaternion.Slerp(Rb.rotation, newRotation, 0.15f);
                }
            }

            if (move.magnitude > 0f)
            {
                lastNonZeroDirection = move.normalized;
            }
        }

        private void StartCharging()
        {
            isCharging = true;
            isDashInputReleased = false;
            chargeTime = 0f;
        }

        private void StopCharging()
        {
            isCharging = false;
            isDashInputReleased = true;
        }

        #endregion

        
        #region Secondary Capacity

        protected override void SecondaryCapacity()
        {
            EnableAttackCollider();
           Invoke(nameof(DisableAttackCollider),1f); 
        }

        private void EnableAttackCollider()
        {
            _attackCollider.enabled = true;
        }

        private void DisableAttackCollider()
        {
            _attackCollider.enabled = false;
        }

      

        #endregion
        
        
    }
}