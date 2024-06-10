using System.Collections.Generic;
using DG.Tweening;
using Intégration.V1.Scripts.Game.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Intégration.V1.Scripts.UI
{
    public class FlowerUI : MonoBehaviour
    {
        public FlowerController FlowerPlayer;
        [SerializeField] private List<Sprite> playerCharacterImages;
        [SerializeField] private List<Sprite> playerDeadCharacterImages;
        [SerializeField] private List<Sprite> CapacityIcons;
        [SerializeField] private List<Image> sunImage;
        [SerializeField] private int sun;
        [SerializeField] private Sprite fullSun;
        [SerializeField] private Sprite emptySun;
        [SerializeField] private Image characterIcon;
        [SerializeField] private Image capacityIcon;
        [SerializeField] private AnimationCurve _animationCurve;


        private void Start()
        {
            capacityIcon.sprite = CapacityIcons[FlowerPlayer.characterIndex];
        }

        private void Update()
        {
            if (FlowerPlayer.isDead)
            {
                characterIcon.sprite = playerDeadCharacterImages[FlowerPlayer.characterIndex];
            }
            else
            {
                characterIcon.sprite = playerCharacterImages[FlowerPlayer.characterIndex];
            }

            if (FlowerPlayer)
            {
                sun = FlowerPlayer.sun;


                if (sun == FlowerPlayer.CapacityCost)
                {
                    capacityIcon.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutSine)
                        .SetLoops(-1, LoopType.Yoyo);
                }
                else
                {
                    //  capacityIcon.transform.DOScale(1, 1);
                }
            }

            for (int i = 0; i < sunImage.Count; i++)
            {
                if (i < sun)
                {
                    sunImage[i].sprite = fullSun;
                    sunImage[i].transform.DOScale(1.2f, 0.5f);
                    // sunImage[i].transform.DOScale(1.2f, -1).SetEase(_animationCurve);
                }
                else
                {
                    sunImage[i].sprite = emptySun;
                    sunImage[i].transform.DOScale(1f, 0.5f);
                }
            }
        }
    }
}