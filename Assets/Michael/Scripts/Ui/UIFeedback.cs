using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michael.Scripts.Ui
{
    public class UIFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Color _buttonColor;
        [SerializeField] private GameObject tutoPanel;
        private static Button _currentButton;
        private void Start()
        {
            _currentButton = null;
        }

        public void OnSelect(BaseEventData eventData) {
            
            // Vérifier si un bouton est déjà sélectionné
            if (_currentButton != null)
            {
                // Désélectionner le bouton précédent
                _currentButton.OnDeselect(eventData);
            }
            
            
            gameObject.GetComponent<Image>().color = Color.white;
            transform.DOScale(1.1f, 0.5f);
            buttonText.color = Color.grey;
            
            if (tutoPanel) {
                tutoPanel.SetActive(true);
            }
            
            _currentButton = GetComponent<Button>();
            
        }

        public void OnDeselect(BaseEventData eventData)
        {
            gameObject.GetComponent<Image>().color = _buttonColor;
            transform.DOScale(1.0f, 0.5f);
            buttonText.color = Color.white;
            
            if (tutoPanel) {
                tutoPanel.SetActive(false);
            }
            
            _currentButton = null;
           
        }
        
        
        

      
    }
}
