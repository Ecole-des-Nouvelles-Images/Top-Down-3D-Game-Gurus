using System.Collections.Generic;
using DG.Tweening;
using Michael.Scripts.Character_Selection;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michael.Scripts.Ui
{
    public class MenuManager : MonoBehaviourSingleton<MenuManager>
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject CharacterSelectionPanel;
        [SerializeField] private GameObject eventSystem;
       
        private void Start()
        {
            _musicSlider.value = DataManager.MusicVolume;
            _sfxSlider.value =  DataManager.SfxVolume;
            SetMusicVolume();
            SetSfxVolume();

            if (DataManager.CharacterSelectionScene) {
                eventSystem.SetActive(false);
                CharacterSelectionPanel.SetActive(true);
                DataManager.CharacterSelectionScene = false;
            }
            else
            {
                mainMenuPanel.SetActive(true);
            }
        }
        
        
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
        
        

        public void SetMusicVolume()
        {
            DataManager.MusicVolume =  _musicSlider.value;
            _mixer.SetFloat("musicVolume",Mathf.Log10( DataManager.MusicVolume)*20);
        }
    
        public void SetSfxVolume()
        {
            DataManager.SfxVolume = _sfxSlider.value;
            _mixer.SetFloat("soundFXVolume",Mathf.Log10( DataManager.SfxVolume)*20);
      
        }

        
        
        
    
    }
}

