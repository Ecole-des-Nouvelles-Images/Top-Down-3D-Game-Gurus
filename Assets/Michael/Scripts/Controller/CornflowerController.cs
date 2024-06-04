using Shields.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Michael.Scripts.Controller
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
            ;
        }
    }
}