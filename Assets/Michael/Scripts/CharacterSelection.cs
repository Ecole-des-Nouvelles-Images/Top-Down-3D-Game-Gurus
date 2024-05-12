using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Michael.Scripts
{
    public class CharacterSelection : MonoBehaviour
    {
        public static bool[] PlayerIsReady;
        public static bool[] PlayerIsJoined;
        public static bool CanStart;
        public int PlayerIndex ;
        public int _maxPlayers ;
        [SerializeField] private List<Button> _characterButtons;
        [SerializeField] private Button _characterSelected;
        [SerializeField] private int _characterIndex;
        [SerializeField] private EventSystem _eventSystem;
        
        
        private void Start() {
            CanStart = false;
            PlayerIsReady = new bool[5] {false, false, false, false,false};
            PlayerIsJoined = new bool[5] {false, false, false, false,false};
        }
        
        void OnNavigate() {  //Bouger le cursor du player 
          PlayerSelector();
        }

        void PlayerSelector() {
            if (_eventSystem.currentSelectedGameObject) {
                transform.SetParent( _eventSystem.currentSelectedGameObject
                    .GetComponentInChildren<HorizontalLayoutGroup>().transform);
            }
        }


        void OnSubmit() { //Valider la selection d'un personnage 

            if (!PlayerIsReady[PlayerIndex])
            {
                if (GetComponentInParent<Button>()) {
                
                    _characterSelected = GetComponentInParent<Button>();
                    PlayerReady();
                    _characterIndex = _characterButtons.IndexOf(_characterSelected);
                    Debug.Log(_characterIndex);
                    _characterSelected.enabled = false;

                    var buttonGrid = _characterSelected.GetComponentInChildren<HorizontalLayoutGroup>();
                    
                
                    if ( buttonGrid.transform.childCount >= 2) {
                        Debug.Log("il a plus de 1 joueur sur ce personnage");
                  
                        for (int i = 0; i < buttonGrid.transform.childCount; i++) {
                            Transform childTransform = buttonGrid.transform.GetChild(i);
                        
                            if (childTransform != this.transform) {
                                Button buttonWithNoChildren = FindButtonWithNoChildren(_characterButtons);
                                childTransform.GetComponentInChildren<EventSystem>()
                                    .SetSelectedGameObject(buttonWithNoChildren.gameObject);
                                childTransform.GetComponent<CharacterSelection>().PlayerSelector();
                            }
                        }
                    }
                }
            }

         
        }
        
        
        Button FindButtonWithNoChildren(List<Button> buttonList) {
            foreach (Button button in _characterButtons) {
                if (button.GetComponentInChildren<HorizontalLayoutGroup>().transform.childCount == 0) {
                    return button;
                }
            }
            return null;
        }
        
        
        
        void OnCancel() { // Annulé la la selection d'un personnage 
            
            Debug.Log("retour");
            if (PlayerIsReady[PlayerIndex]) {
                PlayerIsReady[PlayerIndex] = false;
                _characterSelected.enabled = true;
                Debug.Log("le joueur nest plus pret");
            }
            else if (PlayerIsJoined[PlayerIndex])
            {
               // GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(_joinButton);
               PlayerIsJoined[PlayerIndex] = false;
               Debug.Log("le jooueur est parti");
            }
        }
        


        public void PlayerJoined() {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;
            PlayerIsJoined[PlayerIndex ] = true;
        }
        
        public void PlayerReady() {
            PlayerIsReady[PlayerIndex] = true;
            bool allPlayersReady = true;
            int readyCount = 0;
            Debug.Log( PlayerIsReady[PlayerIndex]);

            for (int i = 0; i < PlayerIsJoined.Length; i++) {
                if (PlayerIsJoined[i] == true) {
                    if (PlayerIsReady[i] == false) {
                        allPlayersReady = false;
                    }
                    else {
                        readyCount++;
                        Debug.Log("le joueur est pret");
                       // GetComponent<EventSystem>().SetSelectedGameObject(null);
                    }
                }
            }
            if (allPlayersReady == true ) {
                CanStart = true;
            }
            else {
                CanStart = false;
            }
        }
        
        
        
        
    }

}
