using System;
using System.Collections.Generic;
using System.Linq;
using Michael.Scripts.Manager;
using UnityEngine;

namespace Michael.Scripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        public List<GameObject> characterPrefabs; 
        public List<Transform> spawnPoints;

        private void Start() { 
            if (DataManager.Instance.PlayerCharacter.Count > 0) {
                
                var ascendingDict = DataManager.Instance.PlayerCharacter.OrderBy(keyValuePair => keyValuePair.Key);
                foreach (var player in ascendingDict) {
                    
                    foreach(KeyValuePair<int, int> items in ascendingDict) {
                        Debug.Log("You have "  + items.Key+ " " + items.Value );
                    }
                    
                    GameObject character = Instantiate(characterPrefabs[player.Value], spawnPoints[player.Value].position,
                        Quaternion.identity);
                        
                    GameManager.Instance.Flowers.Add(character);
                    GameManager.Instance.FlowersAlive.Add(character);
                }
            }
        }
    }
}