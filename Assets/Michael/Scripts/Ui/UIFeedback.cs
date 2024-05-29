using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michael.Scripts.Ui
{
    public class UIFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        
        public void OnSelect(BaseEventData eventData) {
            transform.DOScale(1.1f, 0.5f);
            buttonText.color = Color.grey;
            
            
        }

        public void OnDeselect(BaseEventData eventData) {
            transform.DOScale(1.0f, 0.5f);
            buttonText.color = Color.white;
        }
        
        
        

      
    }
}
