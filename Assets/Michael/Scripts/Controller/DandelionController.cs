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
            
            if (isInvincible) {
                
                _invincibilityTimer += Time.deltaTime;
                gameObject.layer = LayerMask.NameToLayer("Dandelion");
                pollenVfx.Play();
            }
            else {
                gameObject.layer = LayerMask.NameToLayer("Default");
                pollenVfx.Stop();
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
