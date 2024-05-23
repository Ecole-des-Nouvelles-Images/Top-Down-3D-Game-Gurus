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
        public List<GameObject> characterPrefabs; 
        public List<Transform> spawnPoints;
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        

        private void Start() {

            for (int i = 0; i < 4 ; i++)
            {
                if ( DataManager.Instance.PlayerChoice.ContainsKey(i))
                {
                    GameObject character = Instantiate(characterPrefabs[ DataManager.Instance.PlayerChoice[i]], spawnPoints[ DataManager.Instance.PlayerChoice[i]].position,
                        Quaternion.identity,this.gameObject.transform);
                 
                    if (character.CompareTag("Turtle")) { 
                        Debug.Log("turtle ajouté");
                        _targetGroup.AddMember(character.transform,1.1f,2.5f);
                        GameManager.Instance.Turtle = character.gameObject;
                        SeeTroughWall._turtle = character.gameObject;
                    }
                    if (!character.CompareTag("Turtle")) {
                        _targetGroup.AddMember(character.transform,1,2);
                        GameManager.Instance.Flowers.Add(character);
                        GameManager.Instance.FlowersAlive.Add(character);
                        Debug.Log("fleur ajouté");
                    }
                }
            }
        }

        
        
           
        
        

        
        
        
        
        
        
        
        
    }
}


    
  
    