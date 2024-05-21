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
        public List<GameObject> FlowersAlive; 
        public List<GameObject> Flowers; 
        public List<GameObject> TurtleTrap; 
        public Dictionary<GameObject,Transform> _sunOccupiedSpawns = new Dictionary<GameObject, Transform>();
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs;
        [SerializeField] private GameObject SunSpawnsParent;
        [SerializeField] private GameObject firstCamera;
        [SerializeField] private GameObject circularTransition;
        [SerializeField] private GameObject CrashVfx;
        
        void Start()
        {
            circularTransition.transform.DOScale(15, 1);
          
        }

        private void Update()
        {
            if (FlowersAlive.Count <= 0)
            {
                Debug.Log("Turtle WiNNNNNNNN");
            }
            
            
        }


        public void StartGame()
        {
            CrashVfx.SetActive(true);
            InvokeRepeating(nameof(SpawnSun),2,7);
            Invoke("ShowTurtle",1.4f);
        }

        
        
        private void SpawnSun() {
            
            if (_sunSpawnPoints.Length > 0) {
                int sunTospawn = Random.Range(1, _sunSpawnPoints.Length);

                for (int i = 0; i < sunTospawn; i++) {
                    Transform randomSpawnPoint = _sunSpawnPoints[Random.Range(0, _sunSpawnPoints.Length)];
                    if (!_sunOccupiedSpawns.ContainsValue(randomSpawnPoint)) {
                        GameObject Sun = Instantiate(_sunPrefabs, randomSpawnPoint.position, randomSpawnPoint.rotation,SunSpawnsParent.transform);
                        _sunOccupiedSpawns.Add(Sun, randomSpawnPoint);
                    }
                }
            }
        }
        
        private void ShowTurtle() {
            Turtle.SetActive(true);
            firstCamera.SetActive(false);
      
        } 
        
        
        
        
        
        
      
        
    }
}
