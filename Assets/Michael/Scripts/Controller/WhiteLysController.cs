using Michael.Scripts.Manager;
using Michael.Scripts.Ui;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace Michael.Scripts.Controller
{
    public class WhiteLysController : FlowerController
    {
        [SerializeField] private VisualEffect shareEnergy;

        protected override void MainCapacity()
        {
            //give his sun to other flowers
            if (sun > 0)
            {

                shareEnergy.Play();
                foreach (GameObject floweralive in GameManager.Instance.FlowersAlive)
                {
                    if (floweralive != gameObject)
                    {
                        if (floweralive.GetComponent<FlowerController>().sun < 3)
                        {
                            if (floweralive.GetComponent<FlowerController>().GatherEnergy != null)
                            {
                                floweralive.GetComponent<FlowerController>().GatherEnergy.Play();
                            }

                            floweralive.GetComponent<FlowerController>().sun += sun;
                            sun = 0;
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

        public override void OnThirdCapacity(InputAction.CallbackContext context)
        {
            if (canReanimate && !IsStunned && !PauseControlller.IsPaused )
            {
                if (context.started) {
                    isCharging = true;
                   
                }
                else if (context.canceled) {
                    isCharging = false;
                    reanimateTimer = 0;
                }
            }
        }
        
        
    }
}