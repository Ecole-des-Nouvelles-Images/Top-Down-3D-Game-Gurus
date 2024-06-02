using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using UnityEngine;
using UnityEngine.VFX;

namespace Michael.Scripts.Controller
{
  
    public class DandelionController : FlowerController
    {
       [SerializeField] private float _invincibilityTimer;
       [SerializeField] private float invincibilityDuration = 5f;
       [SerializeField] private ParticleSystem pollenVfx;
       private bool isBoosted;
        
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
            
            if (isInvincible && !isBoosted) {
                
                pollenVfx.Play();
                aliveModel.SetActive(false);
                _invincibilityTimer += Time.deltaTime;
                moveSpeed += 20;
                isBoosted = true;

            }
            else {
                
                pollenVfx.Stop();
                aliveModel.SetActive(true);
                isBoosted = false;
                moveSpeed = 525;
            }
           
            
            
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
