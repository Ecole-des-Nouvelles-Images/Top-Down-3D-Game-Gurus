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
        public Dictionary<GameObject,Transform> _sunOccupiedSpawns = new Dictionary<GameObject, Transform>();
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs;
        [SerializeField] private GameObject SunSpawnsParent;
        [SerializeField] private GameObject firstCamera;
        [SerializeField] private GameObject circularTransition;
        [SerializeField] private GameObject CrashVfx;
        [SerializeField] private Transform spawnTurtlePosition;
        void Start() {
            circularTransition.transform.DOScale(15, 1.2f);
            
        }

        private void Update() {
        
            if (FlowersAlive.Count <= 0) {
                Debug.Log("Turtle WiNNNNNNNN");
                FlowersAreDead = true;
            }
            else if (TurtleIsDead) {
                Debug.Log("Flower WiNNNNNNNN");
                
            }
           
            
        }


        public void StartGame() {
            CrashVfx.SetActive(true);
            InvokeRepeating(nameof(SpawnSun),2,8);
            Invoke("TurtleEntrance",1.4f);
        }

        
        
        private void SpawnSun() {
            
            if (_sunSpawnPoints.Length > 0) {
                int sunTospawn = Random.Range(3, 5);

                for (int i = 0; i < sunTospawn; i++) {
                    Transform randomSpawnPoint = _sunSpawnPoints[Random.Range(0, _sunSpawnPoints.Length)];
                    if (!_sunOccupiedSpawns.ContainsValue(randomSpawnPoint)) {
                        
                        GameObject Sun = Instantiate(_sunPrefabs, randomSpawnPoint.position, randomSpawnPoint.rotation,SunSpawnsParent.transform);
                        _sunOccupiedSpawns.Add(Sun, randomSpawnPoint);
                    }
                }
            }
        }
        
        private void TurtleEntrance() {
            CameraShake();
            firstCamera.SetActive(false);
            Turtle.SetActive(true);
        }

    
        public void CameraShake()
        {
            firstCamera.transform.DOShakePosition(1, 0.5f, 10);
        }

     
        
      
        
    }
}
