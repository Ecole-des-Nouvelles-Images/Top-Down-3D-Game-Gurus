using System;
using UnityEngine;

namespace Noah.Scripts
{
    public class FlowerController : CharacterController
    {
        [SerializeField] private bool dead;

        protected void Start()
        {
            dead = false;
        }

        protected override void MainCapacity()
        {
            throw new System.NotImplementedException();
        }

        protected override void SecondaryCapacity()
        {
            throw new System.NotImplementedException();
        }

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