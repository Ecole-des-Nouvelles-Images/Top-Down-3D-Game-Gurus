using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.VFX;

namespace Michael.Scripts.Controller
{
  
    public class DandelionController : FlowerController
    {
       [SerializeField] private float _invincibilityTimer;
       [SerializeField] private float invincibilityDuration = 5f;
       [SerializeField] private ParticleSystem pollenVfx;
       [SerializeField] private bool isBoosted;
       
        
      protected override void Start()
       {
           base.Start();
           _invincibilityTimer = 0;
       }
        protected override void Update()
        {
            base.Update();
            
            if (_invincibilityTimer >= invincibilityDuration) {
                isInvincible = false;
                _invincibilityTimer = 0;
            }

            if (isInvincible)
            {
                _invincibilityTimer += TimeManager.Instance.deltaTime;
                if (!isBoosted && !isDead) {
                
                    pollenVfx.Play();
                    // aliveModel.SetActive(false);
               
                    moveSpeed += 40;
                    isBoosted = true;
                }
                
            }
            else
            {
                pollenVfx.Stop();
                //   aliveModel.SetActive(true);
                isBoosted = false;
                moveSpeed = 525;  
            }
            
         
            /*else if (!isInvincible)
            {
                pollenVfx.Stop();
                //   aliveModel.SetActive(true);
                isBoosted = false;
                moveSpeed = 525;  
            }*/
            
            
            
        }
        protected override void PassiveCapacity()
        {
            
          
        }

        protected override void MainCapacity()
        {
            if (sun >= CapacityCost && !IsPlanted)
            {
                isInvincible = true;
                OnLooseSunCapacity(CapacityCost);
            }
           
        }
    }

}
