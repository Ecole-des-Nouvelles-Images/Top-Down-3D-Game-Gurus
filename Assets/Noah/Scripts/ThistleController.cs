using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class ThistleController : FlowerController
    {
        [SerializeField] private GameObject shield;
        [SerializeField] private float shieldDistance = 1.0f;  // Distance from player to shield

        protected override void MainCapacity()
        {
            shield.SetActive(true);
        }


        protected override void PassiveCapacity()
        {
        }
    }
}