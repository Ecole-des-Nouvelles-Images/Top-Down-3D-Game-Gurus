using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Michael.Scripts.Character_Selection;
using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using Michael.Scripts.Ui;
using Unity.VisualScripting;
using UnityEngine;
using CharacterController = Michael.Scripts.Controller.CharacterController;

namespace Michael.Scripts
{
    public class PlayerSpawner : MonoBehaviourSingleton<PlayerSpawner>
    {
        public List<GameObject> characterPrefabs; 
        public List<Transform> spawnPoints;
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        [SerializeField] private List<FlowerUI> FlowerUis;
        public static bool player1spawned;
        public static bool player2spawned;
        public static bool player3spawned;
        
        private void Start() {

             player1spawned = false;
             player2spawned = false;
             player3spawned = false;
                 
            for (int i = 0; i < 2; i++)
            {
                if ( DataManager.Instance.PlayerChoice.ContainsKey(i))
                {
                    GameObject character = Instantiate(characterPrefabs[ DataManager.Instance.PlayerChoice[i]], spawnPoints[ DataManager.Instance.PlayerChoice[i]].position,
                        Quaternion.identity,this.gameObject.transform);
                    GameManager.Instance.Players.Add(character);
                    
                    
                    if (character.CompareTag("Flower"))
                    {
                       
                        character.GetComponent<FlowerController>().characterIndex = DataManager.Instance.PlayerChoice[i];

                        if (!player1spawned)
                        {
                            FlowerUis[0].FlowerPlayer = character.GetComponent<FlowerController>();
                            FlowerUis[0].GameObject().SetActive(true);
                            player1spawned = true;
                        }
                        else if (!player2spawned)
                        {
                            FlowerUis[1].FlowerPlayer = character.GetComponent<FlowerController>();
                            FlowerUis[1].GameObject().SetActive(true);
                            player2spawned = true;
                        }
                        else if (!player3spawned)
                        {
                            FlowerUis[2].FlowerPlayer = character.GetComponent<FlowerController>();
                            FlowerUis[2].GameObject().SetActive(true);
                            player3spawned = true;
                        }
                      
                        _targetGroup.AddMember(character.transform,1,2);
                        GameManager.Instance.FlowersAlive.Add(character);
                        Debug.Log("fleur ajouté");
                    }
                    if (character.CompareTag("Turtle")) { 
                        Debug.Log("turtle ajouté");
                        _targetGroup.AddMember(character.transform,1.1f,2.5f);
                        GameManager.Instance.Turtle = character.gameObject;
                        SeeTroughWall._turtle = character.gameObject;
                    }
                   
                }
            }
        }

        
        
           
        
        

        
        
        
        
        
        
        
        
    }
}


    
  
    