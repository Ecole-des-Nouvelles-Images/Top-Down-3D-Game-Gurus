using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Manager
{
    [System.Serializable]

    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameObject Turtle;
        public bool TurtleIsDead = false;
        public bool FlowersAreDead = false;
        public List<GameObject> FlowersAlive;
        public List<GameObject> Flowers;
        public List<GameObject> TurtleTrap;
        public Dictionary<GameObject, Transform> _sunOccupiedSpawns = new Dictionary<GameObject, Transform>();
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs;
        [SerializeField] private GameObject SunSpawnsParent;
        [SerializeField] private GameObject firstCamera;
        [SerializeField] private GameObject circularTransition;
        [SerializeField] private GameObject CrashVfx;
        [SerializeField] private Transform spawnTurtlePosition;
        [SerializeField] private GameObject TurtleVictoryPanel;
        [SerializeField] private GameObject FlowersVictoryPanel;
        void Start()
        {
            circularTransition.transform.DOScale(15, 1.2f);

        }

        private void Update()
        {

            if (FlowersAlive.Count <= 0)
            {
                TurtleVictoryPanel.GetComponent<CanvasGroup>().DOFade(1, 1);
                FlowersAreDead = true;
            }
            else if (TurtleIsDead)
            {
             FlowersVictoryPanel.GetComponent<CanvasGroup>().DOFade(1, 1);
            }
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
        }


        public void CameraShake(float duration, float strength, int vibrato)
        {
            firstCamera.transform.DOShakePosition(duration, strength, vibrato);
        }

        public void StartSlomotion()
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        public void FinishSlomotion()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    




    }
}
