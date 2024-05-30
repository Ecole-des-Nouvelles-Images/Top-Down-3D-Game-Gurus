using UnityEngine;

namespace Noah.Scripts
{
    public class CornflowerController : FlowerController
    {
        [SerializeField] Shield _Shield;

        protected override void MainCapacity()
        {
            _Shield.OpenCloseShield();
        }

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        }
    }
}
