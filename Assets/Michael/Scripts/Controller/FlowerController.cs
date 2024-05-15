using System;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts.Controller
{
    public abstract class FlowerController : CharacterController
    {
        public static Action OnSunCollected;
        public int Sun;
        public int MaxSun;
        public bool CanReanimate;
        private enum State
        {
            Alive,
            Planted,
            Stunned,
            Dead
        }

       
        
        [SerializeField] private State CurrentState;
        
        protected void Start()
        {
            CurrentState = State.Alive;
        }
        
        protected override void SecondaryCapacity()
        {
            CurrentState = State.Planted;
            // this.gameObject.SetActive(false);
            // Michael Dig pas besoin d'override
            
        }

        protected override void ThirdCapacity()
        {
            // Michael Reanimate
            Sun = Sun- MaxSun;
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

          

          /*  if (other.CompareTag("Seed"))
            {
                CanReanimate = true;
            }*/
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Sun")) {
              
                CollectSun(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
          /*  if (other.CompareTag("Seed"))
            {
                CanReanimate = false;
            }*/
        }

        private void CollectSun(GameObject sun)
        {
            if (Sun < MaxSun) {
                GameManager.Instance.OnSubCollected(sun);
                Sun++;
            }
        }
        
        
        

        private void GetStunned()
        {
            CurrentState = State.Stunned;
        }

        private void TakeHit()
        {
            CurrentState = State.Dead;
        }
    }
}