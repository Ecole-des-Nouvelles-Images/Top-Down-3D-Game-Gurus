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
            if (sun > 0)
            {
                foreach (GameObject floweralive in  GameManager.Instance.FlowersAlive)
                {
                    floweralive.GetComponent<FlowerController>().sun += sun;
                    sun = 0;
                
                    Debug.Log(floweralive.GetComponent<FlowerController>().sun);
                }
            }
        }

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException(); // Revive cost 1. 
        }

        protected override void ThirdCapacity()
        {
            sun =- 1;
            if (sun < 0) {
                sun = 0;
            }
            canReanimate = false;

        }
        
        
        
    }
}