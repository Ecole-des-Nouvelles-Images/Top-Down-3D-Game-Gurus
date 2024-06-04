using Shields.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Noah.Scripts
{
    public class CornflowerController : FlowerController
    {
        [SerializeField] Shield _Shield;
        [SerializeField] private float _invincibilityTimer;
        [SerializeField] private float invincibilityDuration = 2f;
        protected override void Update()
        {
            base.Update();
            
            if (_invincibilityTimer >= invincibilityDuration) {
                isInvincible = false;
                _invincibilityTimer = 0;
            }
            
            if (isInvincible ) {

                _invincibilityTimer += Time.deltaTime;

            }
          
           
            
            
        }
        protected override void MainCapacity()
        {
            if (sun >= CapacityCost && !IsPlanted)
            {
                isInvincible = true;
                OnLooseSunCapacity(CapacityCost);
                _Shield.OpenShield();
            }
         
        }

        protected override void PassiveCapacity()
        {
            // Si 2 fleurs ou + dans la bulle, repousse la tortue
            
            
            throw new System.NotImplementedException();
        }
    }
}
