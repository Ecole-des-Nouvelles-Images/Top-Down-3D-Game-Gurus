using System;
using System.Collections.Generic;
using Michael.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Michael.Scripts.Ui
{
    public class FlowerUI : MonoBehaviour
    {
        public FlowerController FlowerPlayer;
       [SerializeField] private List<Sprite> playerCharacterImages;
       [SerializeField] private int sun;
       [SerializeField] private List<Image> sunSprites;
       [SerializeField] private  Sprite fullSun;
       [SerializeField] private  Sprite emptySun;

       private void Update()
       {
           sun = FlowerPlayer.sun;
           for (int i = 0; i < sunSprites.Count; i++)
           {
               if (i < sun)
               {
                   sunSprites[i].sprite = fullSun;
               }
               else
               {
                   sunSprites[i].sprite = emptySun;  
               }
           }
           
       }
    }
}