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
                    if (flowerController.sun < flowerController.maxSun) {

                        if (other.GetComponent<CloverController>()) {
                          
                            GameManager.Instance._sunOccupiedSpawns.Remove(gameObject);
                            
                            int rngLuckCost = Random.Range(0, 4);
                            if (rngLuckCost != 1) {
                                other.GetComponent<FlowerController>().sun++ ;
                            }
                            else {
                                other.GetComponent<FlowerController>().sun += 3 ;
                                Debug.Log("chanceux");
                            }
                            _collected = true;
                            Destroy(gameObject);
                        }
                        else
                        {
                            GameManager.Instance._sunOccupiedSpawns.Remove(gameObject);
                            other.GetComponent<FlowerController>().sun++;
                            _collected = true;
                            Destroy(gameObject);
                        }
                        
                    }
                }
            }

            if (other.CompareTag("Turtle"))
            {
                if (!_collected)
                {
                        GameManager.Instance._sunOccupiedSpawns.Remove(gameObject);
                        BatteryManager.Instance.CurrentBatteryTime += 15;
                        _collected = true;
                        Destroy(gameObject);;
                }
            }
        }
    }
}

