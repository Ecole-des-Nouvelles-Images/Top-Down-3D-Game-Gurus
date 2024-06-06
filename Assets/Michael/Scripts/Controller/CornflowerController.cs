using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class CornflowerController : FlowerController
    {
        
        [SerializeField] Shield _Shield;

        protected override void MainCapacity()
        {
            if (sun >= CapacityCost && !IsPlanted)
            {
                _Shield.OpenShield();
                OnLooseSunCapacity(CapacityCost);
            }
            
        }
        

        protected override void PassiveCapacity()
        {
            ;
        }
     
    }
}