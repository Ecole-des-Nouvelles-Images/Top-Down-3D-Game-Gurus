using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public List<GameObject> FlowersAlive;
        public List<GameObject> Flowers;
        [SerializeField] private List<Transform> _sunOccupiedSpawns;
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs; 
        
        
        void Start() {
            InvokeRepeating(nameof(SpawnSun),5,10);
        }
        
        private void SpawnSun() {
            
            
            if (_sunSpawnPoints.Length > 0) {
                int sunTospawn = Random.Range(1, _sunSpawnPoints.Length);

                for (int i = 0; i < sunTospawn; i++) {
                    Transform randomSpawnPoint = _sunSpawnPoints[Random.Range(0, _sunSpawnPoints.Length)];
                    if (!_sunOccupiedSpawns.Contains(randomSpawnPoint)) {
                        Instantiate(_sunPrefabs, randomSpawnPoint.position, randomSpawnPoint.rotation);
                        _sunOccupiedSpawns.Add(randomSpawnPoint);
                    }
                    Debug.Log("spawnSun");
                    
                }
            }
        }
        
        void OnSubCollected(GameObject sun) {
            _sunOccupiedSpawns.Remove(sun.transform);
            Destroy(sun);
        }
        
    }
}
