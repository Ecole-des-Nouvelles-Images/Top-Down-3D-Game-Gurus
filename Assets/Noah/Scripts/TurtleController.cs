using DG.Tweening;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Noah.Scripts
{
    public class TurtleController : CharacterController
    {
        [Header("General References")]
        [SerializeField] private Collider _attackCollider;
        [SerializeField] private GameObject dashTrail;

        [Header("Charging & Dashing")] [SerializeField] private float firstDashLevelTime = 0.7f;
        [SerializeField] private float firstDashLevelPower = 5; 
        [SerializeField] private float secondDashLevelTime = 1.5f, secondDashLevelPower = 10; 
        [SerializeField] private float thirdDashLevelTime = 3f, thirdDashLevelPower = 20;
        private float chargeTime;
        private bool _isCharging;
        private bool _isDashing;
        
        [Header("Scanning")]
        [SerializeField] private float scanTime, scanRange, scanDuration;
        [SerializeField] private GameObject scanSphereArea;
        private bool _isScanning;
        
        [Header("Trap")] 
        [SerializeField] private GameObject TrapPrefab;
        [SerializeField] private Transform TrapSpawn;
        
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
            if (!_isDashing && !_isCharging)
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
            if (!_isDashing)
            {
                Vector3 dashDirection = new Vector3(move.x, 0f, move.y);
                float currentDashForce = 0;
                
                if (chargeTime > firstDashLevelTime && chargeTime < secondDashLevelTime)
                {
                    currentDashForce = firstDashLevelPower * firstDashLevelTime;
                    Debug.Log("First Level Dash");
                }
                else if (chargeTime > firstDashLevelTime && chargeTime > secondDashLevelTime && chargeTime < thirdDashLevelTime)
                {
                    currentDashForce = secondDashLevelPower * secondDashLevelTime;
                    Debug.Log("Second Level Dash");

                }
                else if (chargeTime > firstDashLevelTime && chargeTime > secondDashLevelTime && chargeTime > thirdDashLevelTime)
                {
                    currentDashForce = thirdDashLevelPower * thirdDashLevelTime;
                    Debug.Log("Third Level Dash");
                }
                else
                {
                    Debug.Log("No Force");
                }
                Rb.AddForce(currentDashForce * dashDirection, ForceMode.Impulse);
                _isDashing = true;
                Debug.Log(chargeTime);
                BatteryManager.Instance.BatteryCost(10);
            }
        }


        private void DashingUpdate()
        {
            if (_isDashing && Rb.velocity.magnitude < 0.01f)
            {
                _isDashing = false;
                _animator.SetBool("IsDashing",false);
                dashTrail.SetActive(false);
            }

            if (_isCharging)
            {
                _animator.SetBool("IsDashing",true);
                _animator.SetFloat("DashTimer",chargeTime);
                dashTrail.SetActive(true);
                chargeTime += Time.deltaTime;
            }
        }
        

        private void StartCharging()
        {
            _isCharging = true;
            chargeTime = 0f;
        }

        private void StopCharging()
        {
            _isCharging = false;
        }

        #endregion

        #region Secondary Capacity

        protected override void SecondaryCapacity()
        {
            if (!_isDashing) {
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

            if (GameManager.Instance.TurtleTrap.Count <= 1)
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

            if (!_isScanning)
            {
                scanSphereArea.transform.DOScale(scanRange, 3f);
                _isScanning = true;
                BatteryManager.Instance.BatteryCost(20);
            }
            
        }

        private void ScanningUpdate() {
            if (_isScanning) {
              
                scanTime += Time.deltaTime;
                if (scanTime >= scanDuration) {
                    _isScanning = false;
                    scanTime = 0;
                    scanSphereArea.transform.DOScale(0, 0);
                    
                }
            }
            
        }
        
        #endregion
        
    }
}