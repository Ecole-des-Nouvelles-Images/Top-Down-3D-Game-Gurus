using System;
using UnityEngine;

namespace Noah.Scripts
{
    public class ShieldDetector : MonoBehaviour
    {
        private Michael.Scripts.Controller.TurtleController TurtleController;
        [SerializeField] public float _shieldCounterForce = 5;

        private void Start()
        {
            TurtleController = GetComponent<Michael.Scripts.Controller.TurtleController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shield"))
            {
                int invincibleFlowers = 0;
                FlowerController[] allFlowers = FindObjectsOfType<FlowerController>();
                foreach (FlowerController flower in allFlowers)
                {
                    if (flower.isInvincible)
                    {
                        invincibleFlowers++;
                    }
                }
                if (invincibleFlowers >= 2)
                {
                    Vector3 shieldDirection = (transform.position - other.transform.position).normalized;
                    Vector3 oppositeShieldDirection = shieldDirection;
                    TurtleController.Rb.AddForce(_shieldCounterForce * oppositeShieldDirection, ForceMode.Impulse);
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Shield"))
            {
                int invincibleFlowers = 0;
                FlowerController[] allFlowers = FindObjectsOfType<FlowerController>();
                foreach (FlowerController flower in allFlowers)
                {
                    if (flower.isInvincible)
                    {
                        invincibleFlowers++;
                    }
                }
                if (invincibleFlowers >= 2)
                {
                    Vector3 shieldDirection = (transform.position - other.transform.position).normalized;
                    Vector3 oppositeShieldDirection = shieldDirection;
                    TurtleController.Rb.AddForce(_shieldCounterForce * oppositeShieldDirection, ForceMode.Impulse);
                }
            }
        }
    }
}
