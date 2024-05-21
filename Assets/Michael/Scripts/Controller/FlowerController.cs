using System;
using Michael.Scripts.Manager;
using UnityEditor.AnimatedValues;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Michael.Scripts.Controller
{
    public abstract class FlowerController : CharacterController
    {
        public static Action OnSunCollected;
        public int sun =0 ; 
        public int maxSun = 3 ;
        public bool canReanimate;
        [SerializeField] bool isCharging;
        [SerializeField] float reanimateTimer = 0;
        [SerializeField] private float reanimateDuration = 1;
        [SerializeField] private GameObject deadModel;
        [SerializeField] private GameObject aliveModel;
        [SerializeField] private Collider aliveModelCollider;
        [SerializeField] private bool isDead;
        [SerializeField] private bool isStun;
        [SerializeField] private float stunDuration = 3f;
        [SerializeField] private float stunTimer = 0;
        [SerializeField] private FlowerController deadFlowerController;

        
        protected override void FixedUpdate()
        {
            if (!isStun)
            {
                Move();
            }
        }

        protected override void Update() {

            _animator.SetFloat("Velocity",Rb.velocity.magnitude);
            
            
            if (isCharging) {
                reanimateTimer += Time.deltaTime;
                if (reanimateTimer >= reanimateDuration +0.1) {
                    ThirdCapacity();
                    isCharging = false;
                    reanimateTimer = 0;
                }
            }

            if (isStun)
            {
                stunTimer += Time.deltaTime;
                if (stunTimer >= stunDuration)
                {
                    isStun = false;
                    stunTimer = 0;
                    _animator.SetBool("IsDizzy",false);
                }
            }
            
            
            
            if (Rb.velocity.magnitude > this.idleTreshold && !isDead)
            {
                _animator.SetBool("isPlanted",false);
                aliveModelCollider.enabled = true;
            }
           
            
            
        }        
        protected override void SecondaryCapacity() { // SE PLANTER DANS LE SOL 
            if (!isStun)
            {
                GetPlanted();
            }

            
        }
        public override void OnThirdCapacity(InputAction.CallbackContext context) {// REANIMATION

            if (canReanimate && sun == maxSun && !isStun)
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
            if (sun < 0) {
                sun = 0;
            }
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

     /*   private void CollectSun(GameObject sun)
        {
            if (this.sun < maxSun) {
                
                GameManager.Instance.OnSubCollected(sun);
                this.sun++;
                Debug.Log("soleilszds");            }
        }
*/
        private void GetPlanted() {
            
            _animator.SetBool("isPlanted",true);
            aliveModelCollider.enabled = false;
        }

        private void GetStunned() {
            
            _animator.SetBool("IsDizzy",true);
            isStun = true;
        }
        
        [ContextMenu("TakeHit")]
        private void TakeHit() {
            
            aliveModelCollider.enabled = false;
            aliveModel.SetActive(false);
            deadModel.SetActive(true);
            GetComponent<PlayerInput>().enabled = false;
            isDead = true;
            sun = 0;
            GameManager.Instance.FlowersAlive.Remove(this.gameObject);
        }
        
        [ContextMenu("GetRevive")]
        private void GetRevive() {
            
            aliveModelCollider.enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            isDead = false;
            aliveModel.SetActive(true);
            deadModel.SetActive(false);
            GameManager.Instance.FlowersAlive.Add(this.gameObject);
        }
        
        
        
        
        
    }
}