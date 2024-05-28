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
        [SerializeField] private TurtleUi TurtleUi;
        
        private void Start() {

            for (int i = 0; i < CharacterSelection._maxPlayers ; i++)
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
                        TurtleUi.TurtlePlayer = character.GetComponent<TurtleController>();
                    }
                    if (!character.CompareTag("Turtle"))
                    {
                        character.GetComponent<FlowerController>().characterIndex = DataManager.Instance.PlayerChoice[i];
                        FlowerUis[i].FlowerPlayer = character.GetComponent<FlowerController>();
                        FlowerUis[i].GameObject().SetActive(true);
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


    
  
    