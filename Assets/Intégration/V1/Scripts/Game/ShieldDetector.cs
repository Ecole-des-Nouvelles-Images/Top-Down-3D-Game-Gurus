using Intégration.V1.Scripts.Game.Characters;
using Michael.Scripts.Controller;
using UnityEngine;

namespace Intégration.V1.Scripts.Game
{
    public class ShieldDetector : MonoBehaviour
    {
        TurtleController TurtleController;
        [SerializeField] public float _shieldCounterForce = 5;

        private void Start()
        {
            TurtleController = GetComponent<TurtleController>();
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