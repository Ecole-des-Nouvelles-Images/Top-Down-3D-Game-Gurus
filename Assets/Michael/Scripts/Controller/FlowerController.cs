using System;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts.Controller
{
    public abstract class FlowerController : CharacterController
    {
        private enum State
        {
            Alive,
            Stunned,
            Dead
        }

        public int Sun; 
        
        [SerializeField] private State CurrentState;
        
        protected void Start()
        {
            CurrentState = State.Alive;
        }
        
        protected override void SecondaryCapacity()
        {
            // this.gameObject.SetActive(false);
            // Michael Dig pas besoin d'override
        }

        protected override void ThirdCapacity()
        {
            // Michael Reanimate
            // protected override void ThirdCapacity() pour Lys
        }
        
        protected abstract void PassiveCapacity();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Turtle Collider"))
            {
                TakeHit();
            }

            if (other.CompareTag("Trap"))
            {
                GetStunned();
            }

            if (other.CompareTag("Sun"))
            {
                GameManager.Instance.OnSubCollected(other.gameObject);
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