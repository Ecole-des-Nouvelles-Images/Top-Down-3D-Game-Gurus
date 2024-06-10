using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Intégration.V1.Scripts.Game.FeedBack
{
    public class UISelectionFeedBack : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject OutlineSelector;
        [SerializeField] private AudioSource selectedSound;

        public void OnSelect(BaseEventData eventData)
        {
            OutlineSelector.SetActive(true);
            transform.DOScale(1.1f, 0.5f);
            selectedSound.Play();
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