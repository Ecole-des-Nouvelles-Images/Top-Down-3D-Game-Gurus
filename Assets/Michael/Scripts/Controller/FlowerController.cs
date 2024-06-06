using System;
using DG.Tweening;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Michael.Scripts.Controller
{
    public abstract class FlowerController : CharacterController
    {
        public int characterIndex;
        public static Action OnSunCollected;
        public int sun =0 ; 
        public int maxSun = 3 ;
        public int CapacityCost = 2;
        public bool canReanimate;
        public FlowerController deadFlowerController;
        public bool IsPlanted = false;
        public bool isInvincible = false;
        public bool IsStun;
        public bool isDead;
        [SerializeField] bool isCharging;
        [SerializeField] float reanimateTimer = 0;
        [SerializeField] private float reanimateDuration = 1;
        [SerializeField] private GameObject deadModel;
        [SerializeField] protected GameObject aliveModel;
        [SerializeField] protected Collider aliveModelCollider;
        [SerializeField] private float magnetudeToStun = 22f;
        [SerializeField] private float stunDuration = 3f;
        [SerializeField] private float stunTimer = 0;
        [SerializeField] private ParticleSystem stunParticleSystem;
        [SerializeField] private float plantingCooldown = 0.7f;
        private float currentPlantingCooldown = 0f;
        [SerializeField] private Image reviveChargingIcon;
        [SerializeField] private GameObject deadArrowUI;
       
        protected virtual void Start() {
            StartAnimation();
        }
        private void StartAnimation()
        {
            if (deadArrowUI)
            {
                deadArrowUI.transform.DOLocalMoveY(0.25f, 1 )
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() => {
                        deadArrowUI.transform.DOLocalMoveY(-0.25f, 1 )
                            .SetEase(Ease.InOutSine)
                            .OnComplete(() =>
                            {
                                StartAnimation();
                            });
                    });
            }
         
        }

        protected override void FixedUpdate()
        {
            if (!IsStun)
            {
                Move();
            }
        }

        protected override void Update() {

            _animator.SetFloat("Velocity",Rb.velocity.magnitude);
            
            if (sun < 0) {
                sun = 0;
            }

            if (sun > maxSun) {
                sun = maxSun;
            }

            if (deadFlowerController)
            {
                if (isCharging) {
                    deadFlowerController.reviveChargingIcon.fillAmount = 0;
                    reanimateTimer += Time.deltaTime;
                    DOTween.To(() => deadFlowerController.reviveChargingIcon.fillAmount, value =>
                            deadFlowerController.reviveChargingIcon.fillAmount = value, reanimateTimer / reanimateDuration, reanimateDuration)
                        .OnComplete(() => {
                            
                            if (reanimateTimer >= reanimateDuration + 0.1f) {
                                ThirdCapacity();
                                isCharging = false;
                                reanimateTimer = 0;
                            }
                        });
                }
                else {
                    DOTween.To(() => deadFlowerController.reviveChargingIcon.fillAmount,
                        value => deadFlowerController.reviveChargingIcon.fillAmount = value, 0f, reanimateDuration);
                }

            }
            
          
            

            if (IsStun)
            {
                stunTimer += Time.deltaTime;
                if (stunTimer >= stunDuration)
                {
                    stunParticleSystem.gameObject.SetActive(false);
                    IsStun = false;
                    stunTimer = 0;
                    _animator.SetBool("IsDizzy",false);
                }
            }
            
            if (currentPlantingCooldown > 0)
            {
                currentPlantingCooldown -= Time.deltaTime;
            }

        }  
        
        
        public override void OnSecondaryCapacity(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (!IsStun && currentPlantingCooldown <= 0)
                {
                    if (!IsPlanted)
                    {
                        SecondaryCapacity();
                    }
                    else
                    {
                        GetUnplanted();
                      
                    }
                }
            }
        }
        protected override void SecondaryCapacity() { // SE PLANTER DANS LE SOL 
            
            GetPlanted();
            currentPlantingCooldown = plantingCooldown;
                
        }
        
        private void GetUnplanted()
        {
            if (IsPlanted)
            {
                IsPlanted = false;
                Rb.isKinematic = false;
                _animator.SetBool("isPlanted", IsPlanted);
                aliveModelCollider.enabled = true;
                currentPlantingCooldown = plantingCooldown;
            }
        } 
        
        public override void OnThirdCapacity(InputAction.CallbackContext context) {// REANIMATION

            if (canReanimate && sun == maxSun && !IsStun)
            {
                if (context.started) {
                    isCharging = true;
                   
                }
                else if (context.canceled) {
                    isCharging = false;
                    reanimateTimer = 0;
                }
            }
        }

        protected override void ThirdCapacity() // revive ally 
        {
            Debug.Log("revive");
            sun =- maxSun;
            deadFlowerController.GetRevive();
            canReanimate = false;
            
        }
        
        protected abstract void PassiveCapacity();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Turtle Collider")) {
                TakeHit();
            }
            if (other.CompareTag("TurtleTrap")) {
                
                GameManager.Instance.TurtleTrap.Remove(other.gameObject);
                Destroy(other.gameObject);
                GetStunned();
            }

            if (other.CompareTag("Seed"))
            {
                canReanimate = true;
                deadFlowerController = other.GetComponentInParent<FlowerController>();
                deadFlowerController.reviveChargingIcon.gameObject.SetActive(true);
                deadFlowerController.deadArrowUI.SetActive(false);
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Turtle") && !isDead)
            {
                Rigidbody turtleRb = other.gameObject.GetComponent<Rigidbody>();
                if (turtleRb.velocity.magnitude > magnetudeToStun)
                {
                    GetStunned();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Seed"))
            {
                canReanimate = false;
                isCharging = false;
                reanimateTimer = 0;
                deadFlowerController.reviveChargingIcon.gameObject.SetActive(false);
                deadFlowerController.deadArrowUI.SetActive(true);
                deadFlowerController = null;
            }
        }

        

        private void GetPlanted() {
            
            IsPlanted = true;
            Rb.isKinematic = true;
            _animator.SetBool("isPlanted",IsPlanted);
            aliveModelCollider.enabled = false;
        }

        [ContextMenu("GetStunned")]
        private void GetStunned() {
            
            stunParticleSystem.gameObject.SetActive(true);
            _animator.SetBool("IsDizzy",true);
            IsStun = true;
        }
        
        [ContextMenu("TakeHit")]
        private void TakeHit() {
            if (!isInvincible) {
                aliveModelCollider.enabled = false;
                aliveModel.SetActive(false);
                deadModel.SetActive(true);
                GetComponent<PlayerInput>().enabled = false;
                isDead = true;
                sun = 0;
                GameManager.Instance.FlowersAlive.Remove(this.gameObject);
                deadArrowUI.SetActive(true);
            }
           
        }
        
        [ContextMenu("GetRevive")]
        public void GetRevive() {
            
            aliveModelCollider.enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            isDead = false;
            aliveModel.SetActive(true);
            deadModel.SetActive(false);
            GameManager.Instance.FlowersAlive.Add(this.gameObject);
        }

     

        public void OnLooseSunCapacity(int capacityCost)
        {
            sun -= capacityCost;
        }

        
        
        
    }
}