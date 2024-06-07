using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Manager
{
    [System.Serializable]

    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameObject Turtle;
        public bool TurtleIsDead = false;
        public bool FlowersIsdead = false;
        public bool GameFinished = false;
        public bool GameisStarted = false;
        public List<GameObject> FlowersAlive;
        public List<GameObject> Players;
        public List<GameObject> TurtleTrap;
        public Dictionary<GameObject, Transform> _sunOccupiedSpawns = new Dictionary<GameObject, Transform>();
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs;
        [SerializeField] private GameObject SunSpawnsParent;
        [SerializeField] private GameObject firstCamera;
        [SerializeField] private GameObject circularTransition;
        [SerializeField] private GameObject CrashVfx;
       // [SerializeField] private Transform spawnTurtlePosition;
        [SerializeField] private GameObject EndGamePanel;
        //[SerializeField] private GameObject TurtleVictoryPanel;
        //[SerializeField] private GameObject FlowersVictoryPanel;
        [SerializeField] private GameObject TurtleUis;
        [SerializeField] private GameObject eventSystem;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject okButton;
        [SerializeField] private GameObject RulesPanel;


        void Start()
        {
          //  GameisStarted = false;
          // RulesPanel.GetComponent<CanvasGroup>().DOFade(1, 1f);
          // Invoke(nameof(ShowRulesPanels),5f);
           circularTransition.transform.DOScale(15, 1.2f);

        }
        
        private void DesactiveGameManager()
        {
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerInput>().enabled = false;
            }
        }

        public void Winverification()
        {
            if (FlowersAlive.Count <= 0 && !GameFinished) {
                
                //  TurtleVictoryPanel.SetActive(true);
                EndGamePanel.GetComponent<CanvasGroup>().DOFade(1, 3f);
                eventSystem.SetActive(true);
                eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(restartButton);
                GameFinished = true;
                FlowersIsdead = true;
                DesactiveGameManager();
                
            }
            else if (TurtleIsDead && !GameFinished) {
                // FlowersVictoryPanel.SetActive(true);
                EndGamePanel.GetComponent<CanvasGroup>().DOFade(1, 3f);
                eventSystem.SetActive(true);
                eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(restartButton);
                GameFinished = true;
                TurtleUis.SetActive(false);
                DesactiveGameManager();
                
            }
        }
      

        public void ShowRulesPanels()
        {
            Time.timeScale = 0;
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(okButton);
            
            
        }
        
        public void closeRulesPanels()
        {
            Time.timeScale = 1;
            GameisStarted = true; 
        }
        
        


        public void StartGame()
        {
            CrashVfx.SetActive(true);
            InvokeRepeating(nameof(SpawnSun), 2, 8);
            Invoke("TurtleEntrance", 1.45f);
            
        }



        private void SpawnSun()
        {

            if (_sunSpawnPoints.Length > 0)
            {
                int sunTospawn = Random.Range(3, 5);

                for (int i = 0; i < sunTospawn; i++)
                {
                    Transform randomSpawnPoint = _sunSpawnPoints[Random.Range(0, _sunSpawnPoints.Length)];
                    if (!_sunOccupiedSpawns.ContainsValue(randomSpawnPoint))
                    {

                        GameObject Sun = Instantiate(_sunPrefabs, randomSpawnPoint.position, randomSpawnPoint.rotation,
                            SunSpawnsParent.transform);
                        _sunOccupiedSpawns.Add(Sun, randomSpawnPoint);
                    }
                }
            }
        }

        private void TurtleEntrance()
        {
            CameraShake(1, 0.5f, 10);
            firstCamera.SetActive(false);
            Turtle.SetActive(true);
            TurtleUis.SetActive(true);
            TurtleUis.transform.DOShakePosition(0.5f, 0.1f, 10);
        }


        public void CameraShake(float duration, float strength, int vibrato)
        {
            firstCamera.transform.DOShakePosition(duration, strength, vibrato);
        }

      public void StartSlomotion()
        {
            Time.timeScale = 0.5f;
            TimeManager.Instance.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        public void FinishSlomotion()
        {
            Time.timeScale = 1f;
            TimeManager.Instance.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}
