using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public abstract class FlowerController : CharacterController
    {
        private enum State
        {
            Alive,
            Stunned,
            Dead
        }
        
        [SerializeField] private State currentState;
        
        protected virtual void Start()
        {
            currentState = State.Alive;
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
        }

        private void GetStunned()
        {
            currentState = State.Stunned;
        }

        private void TakeHit()
        {
            currentState = State.Dead;
        }
    }
}