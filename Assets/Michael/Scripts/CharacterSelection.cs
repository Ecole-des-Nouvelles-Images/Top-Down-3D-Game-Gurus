using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Manager;
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
        [SerializeField] private Button _joinButton;
        [SerializeField] private int _characterIndex; // l'index du personnage selectionné
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
            if  (_eventSystem.currentSelectedGameObject.GetComponentInChildren<HorizontalLayoutGroup>()) {
                transform.SetParent( _eventSystem.currentSelectedGameObject
                    .GetComponentInChildren<HorizontalLayoutGroup>().transform);
            }
        }


        void OnSubmit() { //Valider la selection d'un personnage 

            if (!PlayerIsReady[PlayerIndex])
            {
                if (GetComponentInParent<Button>()) {
                
                    _characterSelected = GetComponentInParent<Button>();
                    _characterIndex = _characterButtons.IndexOf(_characterSelected);
                    PlayerReady();
                    Debug.Log("l'index du personnage selectionné est " + _characterIndex);

                    MooveOtherSelectorPosition();
                    
                
                   
                }
            }

         
        }

        void MooveSelectorPosition() {
            if (_characterSelected) {
                var buttonGrid =   _characterSelected.GetComponentInChildren<HorizontalLayoutGroup>();
                if (buttonGrid.transform.childCount >= 2) {
                    Button buttonWithNoChildren = FindButtonWithNoChildren(_characterButtons);
                    _eventSystem.SetSelectedGameObject(buttonWithNoChildren.gameObject);
                    PlayerSelector();
                }
            }
        }
        
        void MooveOtherSelectorPosition() {
            
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
                RemoveChoice(PlayerIndex);
                Debug.Log( DataManager.Instance.PlayerCharacter);
            }
            else if (PlayerIsJoined[PlayerIndex])
            {
              _eventSystem.SetSelectedGameObject(_joinButton.gameObject);
               PlayerIsJoined[PlayerIndex] = false;
               Debug.Log("le jooueur est parti");
            }
        }
        


        public void PlayerJoined() {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;
            PlayerIsJoined[PlayerIndex ] = true;
            _eventSystem.SetSelectedGameObject(_characterButtons[0].gameObject);
           // MooveSelectorPosition();
            Debug.Log("un joueur a rejoin");
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
                        _characterSelected.enabled = false;
                        ConfirmChoice(PlayerIndex,_characterIndex);
                        
                        Debug.Log("le joueur " + PlayerIndex +"est pret");
                        Debug.Log(readyCount+" joueurs pret");
                        foreach(var key in   DataManager.Instance.PlayerCharacter.Keys)
                        {
                            Debug.Log($"Key: {key}, Value: {   DataManager.Instance.PlayerCharacter[key]}");
                        }
                    }
                }
            }
            if  (/*allPlayersReady == true && readyCount > _maxPlayers*/ readyCount >=2 ) {
                CanStart = true;
            }
            else {
                CanStart = false;
            }

            if (CanStart)
            {
                CustomSceneManager.Instance.LoadScene("Game");
            }
        }
        public void ConfirmChoice(int playerIndex, int characterIndex) {

            if (!DataManager.Instance.PlayerCharacter.ContainsKey(PlayerIndex))
            {
                DataManager.Instance.PlayerCharacter.Add(playerIndex, characterIndex);
            }
            
          
        }
        public void RemoveChoice(int playerIndex) {
            DataManager.Instance.PlayerCharacter.Remove(playerIndex);
        }
        
        
        
        
        
    }

}
