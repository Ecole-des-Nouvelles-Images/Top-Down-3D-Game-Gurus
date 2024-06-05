using System;
using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Michael.Scripts.Ui
{
    public class PauseControlller : MonoBehaviourSingleton<PauseControlller>
    {
        public static bool IsPaused;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _eventSystem;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private Volume volume;

        private void Start()
        {
            IsPaused = false;
            TimeManager.Instance.timeScale = 1;
        }


        public void OpenPausePanel() {
            if ( TimeManager.Instance.timeScale > 0)
            {
                TimeManager.Instance.timeScale = 0;
                IsPaused = true;
                _pausePanel.SetActive(true);
                _eventSystem.SetActive(true);
                _eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(_playButton);
            }
        }
        
        
        public void ClosePausePanel() {
            if (TimeManager.Instance.timeScale <= 0)
            { 
                TimeManager.Instance.timeScale = 1;
                IsPaused = false;
               // _eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                _pausePanel.SetActive(false);
                _eventSystem.SetActive(false);

            }
        }
        
        
    }
}

