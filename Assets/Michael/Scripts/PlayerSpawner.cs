using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Michael.Scripts.Manager;
using UnityEngine;

namespace Michael.Scripts
{
    public class PlayerSpawner : MonoBehaviourSingleton<PlayerSpawner>
    {
      //  public List<GameObject> characterPrefabs; 
        public List<Transform> spawnPoints;
        [SerializeField] private CinemachineTargetGroup _targetGroup;

        private void Start()
        {
            
            foreach (GameObject characterPrefabs in DataManager.Instance.PlayerChoice) {
                if (characterPrefabs != null)
                {
                    GameObject character = Instantiate(characterPrefabs, spawnPoints[DataManager.Instance.CharacterPrefabs.IndexOf(characterPrefabs)].position,
                        Quaternion.identity,this.gameObject.transform);
                    
                
                    if (character.CompareTag("Turtle")) { 
                        Debug.Log("turtle ajouté");
                        _targetGroup.AddMember(character.transform,1.1f,2.5f);
                        GameManager.Instance.Turtle = character.gameObject;
                        SeeTroughWall._turtle = character.gameObject;
                        
                    }
                    if (!character.CompareTag("Turtle"))
                    {
                        _targetGroup.AddMember(character.transform,1,2);
                        GameManager.Instance.Flowers.Add(character);
                        GameManager.Instance.FlowersAlive.Add(character);
                        Debug.Log("fleur ajouté");
                    }
                }


            }
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
             //   var ascendingDict = DataManager.Instance.PlayerCharacter.OrderBy(keyValuePair => keyValuePair.Key);
              
               /* foreach (var player in ascendingDict) {
                    
                    foreach(KeyValuePair<int, int> items in ascendingDict) {
                        Debug.Log("You have "  + items.Key+ " " + items.Value );
                    }
                    GameObject character = Instantiate(characterPrefabs[player.Value], spawnPoints[player.Value].position,
                        Quaternion.identity,this.gameObject.transform);
                    
                  
                    if (character.CompareTag(characterPrefabs[6].tag)) {
                        Debug.Log("turtle ajouté");
                        _targetGroup.AddMember(character.transform,1.1f,2.5f);
                        GameManager.Instance.Turtle = character.gameObject;
                        SeeTroughWall._turtle = character.gameObject;
                        
                    }
                    if (character.tag != characterPrefabs[6].tag)
                    {
                        _targetGroup.AddMember(character.transform,1,2);
                        GameManager.Instance.Flowers.Add(character);
                        GameManager.Instance.FlowersAlive.Add(character);
                        Debug.Log("fleur ajouté");
                    }*/
                
        }
    }
}
    
  
    