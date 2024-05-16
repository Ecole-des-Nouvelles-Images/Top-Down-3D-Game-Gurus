using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    [System.Serializable]
   
    public class GameManager : MonoBehaviourSingleton<GameManager>
    { 
        public List<GameObject> FlowersAlive; 
        public List<GameObject> Flowers;
        //[SerializeField] private List<Transform> _sunOccupiedSpawns;
        [SerializeField] private Dictionary<GameObject,Transform> _sunOccupiedSpawns = new Dictionary<GameObject, Transform>();
        [SerializeField] private Transform[] _sunSpawnPoints;
        [SerializeField] private GameObject _sunPrefabs;
        [SerializeField] private GameObject SunSpawnsParent;
        
        void Start() {
            InvokeRepeating(nameof(SpawnSun),5,10);
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
        
        public void OnSubCollected(GameObject sun) {
            _sunOccupiedSpawns.Remove(sun);
            Destroy(sun);
        }
        
    }
}
