using System.Collections.Generic;
using Michael.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Michael.Scripts
{
    public class CharacterSelection : MonoBehaviour
    {
        public static bool[] PlayerIsReady;
        public static bool[] PlayerIsJoined;
        public static bool CanStart;
        public static bool TurtleIsSelected;
        public int PlayerIndex ;
        public int _maxPlayers ;
        [SerializeField] private List<Button> _characterButtons;
        [SerializeField] private Button _characterSelected;
        [SerializeField] private Button _joinButton;
        [SerializeField] private int _characterIndex; // l'index du personnage selectionné
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private GameObject Selector;
        public TextMeshProUGUI joinText;
        
        private void Start() {
            Selector.SetActive(false);
            CanStart = false;
            TurtleIsSelected = false;
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

            
            if (!PlayerIsJoined[PlayerIndex]) {
                PlayerJoined();
            }
            else if (!PlayerIsReady[PlayerIndex]) {
                if (GetComponentInParent<Button>()) {
                    _characterSelected = GetComponentInParent<Button>();
                    _characterIndex = _characterButtons.IndexOf(_characterSelected);
                    _characterSelected.enabled = false;
                    PlayerReady();
                    Debug.Log("l'index du personnage selectionné est " + _characterIndex);
                    MooveOtherSelectorPosition();
                }
            }
        }

        void MooveSelectorPosition() {
            
            Button buttonWithNoChildren = FindButtonWithNoChildren(_characterButtons);
            _eventSystem.SetSelectedGameObject(buttonWithNoChildren.gameObject);
            PlayerSelector();
                
            
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
            
           
            if (PlayerIsReady[PlayerIndex]) {
                Debug.Log("retour");
                PlayerIsReady[PlayerIndex] = false;
                _characterSelected.enabled = true;
                Debug.Log("le joueur nest plus pret");
                RemoveChoice(PlayerIndex);
                joinText.text = "Ready?";
                
                if (_characterSelected.name == "TurtleButton")
                {
                    TurtleIsSelected = false;
                    Debug.Log("turtle deselectionnée");
                }
            }
            else if (PlayerIsJoined[PlayerIndex])
            { 
                joinText.text = "Join";
              _eventSystem.SetSelectedGameObject(_joinButton.gameObject);
              if (_characterSelected) {
                  _characterSelected.enabled = true;
              }
              PlayerIsJoined[PlayerIndex] = false;
              Debug.Log("le jooueur est parti");
              Selector.SetActive(false);
            }
        }
        


        public void PlayerJoined() {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;
            PlayerIsJoined[PlayerIndex ] = true;
            MooveSelectorPosition();
            Debug.Log("un joueur a rejoin");
            Selector.SetActive(true);
            joinText.text = "Ready?";
        }
        
        public void PlayerReady() {
            joinText.text = "READY";
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
                        ConfirmChoice(PlayerIndex,_characterIndex);
                        
                        if (_characterSelected.name == "TurtleButton")
                        {
                            TurtleIsSelected = true;
                            Debug.Log("turtle selectionnée");
                        }
                        Debug.Log("le joueur " + PlayerIndex +"est pret");
                        Debug.Log(readyCount+" joueurs pret");
                        foreach(var key in   DataManager.Instance.PlayerCharacter.Keys)
                        {
                            Debug.Log($"Key: {key}, Value: {   DataManager.Instance.PlayerCharacter[key]}");
                        }
                    }
                }
            }
            if  (/*allPlayersReady == true && readyCount > _maxPlayers*/ readyCount >= 2 && TurtleIsSelected) {
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
