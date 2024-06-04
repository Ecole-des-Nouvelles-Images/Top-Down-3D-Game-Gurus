using Shields.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Noah.Scripts
{
    public class CornflowerController : FlowerController
    {
        [SerializeField] Shield _Shield;

        protected override void MainCapacity()
        {
            _Shield.OpenShield();
        }

        protected override void PassiveCapacity()
        {
            // Si 2 fleurs ou + dans la bulle, repousse la tortue
            
            
            throw new System.NotImplementedException();
        }
    }
}
