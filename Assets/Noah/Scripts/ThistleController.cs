using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class ThistleController : FlowerController
    {
        private Vector2 _moveShield;
        [SerializeField] private GameObject shield;
        [SerializeField] private float shieldDistance = 1.0f;  // Distance from player to shield

        protected override void MainCapacity()
        {
            shield.SetActive(true);
        }

        private void OnShieldJoystick(InputAction.CallbackContext context)
        {
            _moveShield = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            ShieldJoystick();
        }

        private void ShieldJoystick()
        {
            if (_moveShield != Vector2.zero)
            {
                Vector3 shieldDirection = new Vector3(_moveShield.x, 0, _moveShield.y).normalized; 
                Vector3 shieldPosition = transform.position + shieldDirection * shieldDistance;
                shield.transform.position = shieldPosition;
            }
        }

        protected override void PassiveCapacity()
        {
        }
    }
}