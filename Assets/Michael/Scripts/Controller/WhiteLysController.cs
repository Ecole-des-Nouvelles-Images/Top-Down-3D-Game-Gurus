using Michael.Scripts.Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class WhiteLysController : FlowerController
    {
        protected override void MainCapacity()
        {
            //give his sun to other flowers
            if (sun > 0) {
                
                foreach (GameObject floweralive in  GameManager.Instance.FlowersAlive) {
                    if (floweralive != gameObject)
                    {
                        if (floweralive.GetComponent<FlowerController>().sun < 3)
                        {
                            floweralive.GetComponent<FlowerController>().sun += sun;
                            sun = 0;
                        }
                        else
                        {
                            Debug.Log("toutes les fleurs ont des soleils");
                        }
                    }
                }
            
            }
        }

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException(); // Revive cost 1. 
        }

        protected override void ThirdCapacity()
        {
          /*  sun =- 1;
            if (sun < 0) {
                sun = 0;
                
            }*/
            deadFlowerController.GetRevive();
            canReanimate = false;

        }
        
        
        
    }
}