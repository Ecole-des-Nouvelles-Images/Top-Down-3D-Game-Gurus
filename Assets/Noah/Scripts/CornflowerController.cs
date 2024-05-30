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
            throw new System.NotImplementedException();
        }
    }
}
