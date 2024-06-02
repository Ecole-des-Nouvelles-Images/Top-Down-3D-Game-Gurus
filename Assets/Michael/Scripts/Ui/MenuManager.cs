using System.Collections.Generic;
using DG.Tweening;
using Michael.Scripts.Character_Selection;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michael.Scripts.Ui
{
    public class MenuManager : MonoBehaviourSingleton<MenuManager>
    {
       
        public void QuitApplication()
        {
            Application.Quit();
        }

        public void FadeInPanel(GameObject panel)
        {
            panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }

        public void FadeOutPanel(GameObject panel)
        {
            panel.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        }

        
        public void SelectButton(Button newButton)
        {
            // Définir le bouton sélectionné par défaut sur null
            EventSystem.current.SetSelectedGameObject(null);

            // Sélectionner le nouveau bouton
            newButton.Select();
        }
    
    }
}

