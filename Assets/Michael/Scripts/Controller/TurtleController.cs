using DG.Tweening;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Michael.Scripts.Controller
{
    public class TurtleController : CharacterController
    {
        [Header("Trap")] [SerializeField] private GameObject TrapPrefab;
        [SerializeField] private Transform TrapSpawn;

        [Header("Dashing")] [SerializeField] private float dashForce;
        [SerializeField] private float maxDashForce;
        [SerializeField] private bool isDashing;
        [SerializeField] private bool isScanning;
        [SerializeField] private bool isCharging;
        [SerializeField] private float chargeDashMultiplier;
        [SerializeField] private float chargeTime;
        [SerializeField] private float maxChargeTime;
        [SerializeField] private Collider _attackCollider;
        [SerializeField] private GameObject dashTrail;
        [SerializeField] private float scanTime = 0;
        [SerializeField] private float scanDuration = 3;
        [SerializeField] private GameObject scanSphereArea;
        [SerializeField] private float scanRange = 10;
        [SerializeField] private float battery; 
        
        private void Start()
        {
            QteManager.Instance.OnQteFinished += AnimationDash;
            _attackCollider.enabled = false;
        }

        private void AnimationDash()
        {
            _animator.SetBool("QteSuccess",true);
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
            ScanningUpdate();
            _animator.SetFloat("Velocity",Rb.velocity.magnitude);
            
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
                
                BatteryManager.Instance.BatteryCost(10);
            }
        }

        private void DashingUpdate()
        {
            if (isDashing && Rb.velocity.magnitude < 0.01f)
            {
                isDashing = false;
                _animator.SetBool("IsDashing",false);
                dashTrail.SetActive(false);
            }

            if (isCharging)
            {
                _animator.SetBool("IsDashing",true);
                _animator.SetFloat("DashTimer",chargeTime);
                dashTrail.SetActive(true);
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
            if (!isDashing) {
                EnableAttackCollider();
                Invoke(nameof(DisableAttackCollider), 0.7f);
                _animator.SetTrigger("Attack");
                BatteryManager.Instance.BatteryCost(10);
            }
         
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

        protected override void ThirdCapacity() {

            if ( GameManager.Instance.TurtleTrap.Count <= 1)
            {
                GameObject trap = Instantiate(TrapPrefab,TrapSpawn.position,TrapSpawn.rotation);
                GameManager.Instance.TurtleTrap.Add(trap);
                BatteryManager.Instance.BatteryCost(10);
            }
            else
            {
                Debug.Log("2 trap maximum");
            }
          
        }

        #endregion
        
        
        
        
        #region Fourth Capacity

        protected override void FourthCapacity() { //SCANNER LES FLEURS

            if (!isScanning)
            {
                scanSphereArea.transform.DOScale(scanRange, 3f);
                isScanning = true;
                BatteryManager.Instance.BatteryCost(20);
            }
            
        }

        private void ScanningUpdate() {
            if (isScanning) {
              
                scanTime += Time.deltaTime;
                if (scanTime >= scanDuration) {
                    isScanning = false;
                    scanTime = 0;
                    scanSphereArea.transform.DOScale(0, 0);
                    
                }
            }
            
        }
        
        

        #endregion
        
    }
}