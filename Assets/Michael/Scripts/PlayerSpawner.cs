using System;
using System.Collections.Generic;
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
                foreach (var player in DataManager.Instance.PlayerCharacter) {
                    
                    foreach(KeyValuePair<int, int> items in DataManager.Instance.PlayerCharacter) {
                        Debug.Log("You have "  + items.Key+ " " + items.Value );
                    }
                    
                    GameObject character = Instantiate(characterPrefabs[player.Value], spawnPoints[player.Key].position,
                        Quaternion.identity);
                        
                    GameManager.Instance.Flowers.Add(character);
                    GameManager.Instance.FlowersAlive.Add(character);
                }
            }
        }
    }
}