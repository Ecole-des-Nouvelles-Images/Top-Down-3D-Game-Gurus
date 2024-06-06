using System;
using UnityEngine;
using UnityEngine.Video;

namespace Michael.Scripts.Manager
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayerIntro;

        private void Start()
        {
            _videoPlayerIntro.loopPointReached += LoadMenuScene;
        }

        public void LoadMenuScene(VideoPlayer videoPlayer) {
            CustomSceneManager.Instance.LoadScene("Menu");
         }

           
       
    }
}
