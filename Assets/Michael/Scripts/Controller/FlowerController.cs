using System;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private Collider aliveModelCollider;
        [SerializeField] private float magnetudeToStun = 22f;
        [SerializeField] private float stunDuration = 3f;
        [SerializeField] private float stunTimer = 0;
        [SerializeField] private ParticleSystem stunParticleSystem;

        protected virtual void Start() {
            
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
            
            
            
            if (isCharging) {
                reanimateTimer += Time.deltaTime;
                if (reanimateTimer >= reanimateDuration +0.1) {
                    ThirdCapacity();
                    isCharging = false;
                    reanimateTimer = 0;
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
            
            
            
            if (Rb.velocity.magnitude > this.idleTreshold && !isDead)
            {
                IsPlanted = false;
                _animator.SetBool("isPlanted",IsPlanted);
                aliveModelCollider.enabled = true;
            }
           
            
            
        }        
        protected override void SecondaryCapacity() { // SE PLANTER DANS LE SOL 
            if (!IsStun)
            {
                GetPlanted();
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
            if (other.CompareTag("Seed")) {
                canReanimate = true;
                deadFlowerController = other.GetComponentInParent<FlowerController>();
            }
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
                deadFlowerController = null;
            }
        }

        

        private void GetPlanted() {
            
            IsPlanted = true;
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