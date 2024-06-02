using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Michael.Scripts.Ui
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        private PlayerUI _playerUI;
        private void Awake()
        {
            _playerUI = new PlayerUI();
            _playerUI.UI.Cancel.performed += OnBack;
        }
        
        private void OnEnable()
        {
            _playerUI.UI.Enable();
        }

        private void OnDisable()
        {
            _playerUI.UI.Disable();
        }
        
        private void OnBack(InputAction.CallbackContext context)
        {
            if (_backButton != null)
            {
                _backButton.onClick?.Invoke();
            }

        }
        
        
        
        
        
        
        
    }
}