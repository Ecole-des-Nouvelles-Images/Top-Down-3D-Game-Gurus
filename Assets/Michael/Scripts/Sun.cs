using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using UnityEngine;

namespace Michael.Scripts
{
    public class Sun : MonoBehaviour
    {
        private bool _collected = false;
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Flower"))
            {
                if (!_collected)
                {
                    FlowerController flowerController = other.GetComponent<FlowerController>();
                    if (flowerController.sun < flowerController.maxSun)
                    {
                        GameManager.Instance._sunOccupiedSpawns.Remove(gameObject);
                        other.GetComponent<FlowerController>().sun++;
                        _collected = true;
                        Destroy(gameObject);
                    }
                }
             
            }

            if (other.CompareTag("Turtle"))
            {
                
            }
        }
    }
}

