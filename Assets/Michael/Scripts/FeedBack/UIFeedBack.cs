using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Michael.Scripts.FeedBack
{
    public class UIFeedBack : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject OutlineSelector;
    

        public void OnSelect(BaseEventData eventData)
        {
            OutlineSelector.SetActive(true);
            transform.DOScale(1.1f, 0.5f);
        }

        public void OnDeselect(BaseEventData eventData)
        {

            if (GetComponentInChildren<HorizontalLayoutGroup>().transform.childCount <= 1)
            {
                OutlineSelector.SetActive(false);
                transform.DOScale(1.0f, 0.5f);
            }
       
        }
    }


}


