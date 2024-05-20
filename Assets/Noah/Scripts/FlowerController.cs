using System;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
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
        
        private enum State {
            Alive,
            Planted,
            Stunned,
            Dead
        }
        [SerializeField] private State CurrentState;
        
        protected virtual void Start() {
            CurrentState = State.Alive;
        }

        protected override void Update() {
            
            if (isCharging) {
                reanimateTimer += Time.deltaTime;
                if (reanimateTimer >= reanimateDuration +0.1) {
                    ThirdCapacity();
                    isCharging = false;
                    reanimateTimer = 0;
                }
            }
            
            
            //pour l'animation de course 
            if (Rb.velocity.magnitude > this.idleTreshold)
            {
                _animator.SetBool("Run", true);
                _animator.SetBool("IsPlanted",false);
            }
            else
            {
                _animator.SetBool("Run", false); 
            }
            
            
        }        
        protected override void SecondaryCapacity() {
            CurrentState = State.Planted;
            _animator.SetBool("IsPlanted",true);
            // this.gameObject.SetActive(false);
            // Michael Dig pas besoin d'override
        }
        public override void OnThirdCapacity(InputAction.CallbackContext context) {

            if (canReanimate && sun == maxSun)
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
            canReanimate = false;

            // protected override void ThirdCapacity() pour Lys
        }
        
        protected abstract void PassiveCapacity();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Turtle Collider")) {
                TakeHit();
            }

            if (other.CompareTag("Trap")) {
                GetStunned();
            }
            
            if (other.CompareTag("Seed"))
            {
                canReanimate = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Sun")) {
              
                CollectSun(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Seed"))
            {
                canReanimate = false;
                isCharging = false;
                reanimateTimer = 0;
            }
        }

        private void CollectSun(GameObject sun)
        {
            if (this.sun < maxSun) {
                
                GameManager.Instance.OnSubCollected(sun);
                this.sun++;
            }
        }
        
        private void GetStunned()
        {
            CurrentState = State.Stunned;
        }

        private void TakeHit()
        {
            aliveModelCollider.enabled = false;
            aliveModel.SetActive(false);
            deadModel.SetActive(true);
            GetComponent<PlayerInput>().enabled = false;
            CurrentState = State.Dead;
        }
    }
}