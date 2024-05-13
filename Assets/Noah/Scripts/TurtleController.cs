using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class TurtleController : CharacterController
    {
        [SerializeField] private float dashForce;

        
        protected override void MainCapacity()
        {
            // Vector3 forceToApply = transform.forward * dashForce + transform.up * dashUpwardForce;
            // Rb.AddForce(forceToApply, ForceMode.Impulse);
            Vector3 dashDirection = transform.forward;
            Rb.velocity = dashDirection * dashForce;
        }
    }
}