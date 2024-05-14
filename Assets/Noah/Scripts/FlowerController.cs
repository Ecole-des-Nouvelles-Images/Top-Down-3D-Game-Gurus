using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public abstract class FlowerController : CharacterController
    {
        [SerializeField] private bool dead;

        protected void Start()
        {
            dead = false;
        }

        protected abstract void PassiveCapacity();

        #region Non-Used Capacities
        
        protected override void SecondaryCapacity()
        {
            throw new System.NotImplementedException();
        }

        public override void OnThirdCapacity(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        protected override void ThirdCapacity()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Turtle Collider"))
            {
                TakeHit();
            }
        }

        private void TakeHit()
        {
            dead = true;
        }
    }
}