using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts.Controller
{
    public class TurtleController : CharacterController
    {
        [Header("Trap")] [SerializeField] private GameObject TrapPrefab;
        [SerializeField] private Transform TrapSpawn;

        [Header("Dashing")] [SerializeField] private float dashForce;
        [SerializeField] private float maxDashForce;
        [SerializeField] private bool isDashing;
        [SerializeField] private bool isCharging;
        [SerializeField] private float chargeDashMultiplier;
        [SerializeField] private float chargeTime;
        [SerializeField] private float maxChargeTime;
        [SerializeField] private Collider _attackCollider;
        

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
            if (!isDashing)
            {
                Vector3 dashDirection = transform.forward;
                float currentDashForce = Mathf.Clamp(dashForce * (chargeTime * chargeDashMultiplier), 0f, maxDashForce);

                Rb.AddForce(currentDashForce * dashDirection, ForceMode.Impulse);
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
                    Quaternion newRotation = Quaternion.LookRotation(new Vector3(move.x, 0f, move.y), Vector3.up);
                    Rb.rotation = Quaternion.Slerp(Rb.rotation, newRotation, 0.15f);
                }
            }
        }

        private void StartCharging()
        {
            isCharging = true;
            chargeTime = 0f;
        }

        private void StopCharging()
        {
            isCharging = false;
        }

        #endregion

        #region Secondary Capacity

        protected override void SecondaryCapacity()
        {
            EnableAttackCollider();
            Invoke(nameof(DisableAttackCollider), 1f);
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

        #region Third Capacity

        protected override void ThirdCapacity()
        {
            Instantiate(TrapPrefab);
        }

        #endregion
    }
}