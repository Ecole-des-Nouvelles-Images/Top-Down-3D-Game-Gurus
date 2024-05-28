using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class ThistleController : FlowerController
    {
        [SerializeField] private GameObject shield;

        protected override void MainCapacity()
        {
            shield.SetActive(true);
        }


        protected override void PassiveCapacity()
        {
        }
    }
}