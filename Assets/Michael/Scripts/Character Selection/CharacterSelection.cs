using System.Collections.Generic;
using DG.Tweening;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Michael.Scripts.Character_Selection
{
    public class CharacterSelection : MonoBehaviour
    {
        public static bool[] PlayerIsReady;
        public static bool[] PlayerIsJoined;
        public static bool CanStart;
        public static bool TurtleIsSelected;
        public int PlayerIndex ;
        public static int _maxPlayers = 2;
        [SerializeField] private List<Button> _characterButtons;
        [SerializeField] private List<Sprite> _characterSprites;
        [SerializeField] private List<Sprite> _characterCapacitiesSprites;
        [SerializeField] private Button _characterSelected;
        [SerializeField] private Button _joinButton;
        [SerializeField] private int _characterIndex; // l'index du personnage selectionné
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private GameObject Selector;
        [SerializeField] private GameObject joinedText;
        [SerializeField] private Image CapacityImage;
        [SerializeField] private GameObject readyText;
        [SerializeField] private GameObject circleTransition;
        [SerializeField] private GameObject canvas;
        [SerializeField] private Camera camera;
        private Vector3 _initialTransform;
        
        private void Start()
        {
            PlayerIndex = GetComponent<PlayerInput>().user.index;
            _initialTransform = transform.localScale;
            Selector.SetActive(false);
            CanStart = false;
            TurtleIsSelected = false;
            PlayerIsReady = new bool[5] {false, false, false, false,false};
            PlayerIsJoined = new bool[5] {false, false, false, false,false};
        }

        private void Update()
        {
           
        }
      
        
        
        

        void OnNavigate() {  //Bouger le cursor du player 
            PlayerSelector();
        }

        void PlayerSelector() {
            if  (_eventSystem.currentSelectedGameObject.GetComponentInChildren<HorizontalLayoutGroup>()) {
                transform.SetParent( _eventSystem.currentSelectedGameObject
                    .GetComponentInChildren<HorizontalLayoutGroup>().transform);
                _characterSelected = GetComponentInParent<Button>(); 
                _joinButton.image.sprite = _characterSprites[_characterButtons.IndexOf(_characterSelected)];
               

                if (_characterSelected.name == "TurtleButton")
                {
                    CapacityImage.gameObject.SetActive(false);
                    _joinButton.transform.DOScale(1.2f, 0.5f);
                    transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                }
                else if (_characterSelected.name != "TurtleButton")
                {
                    CapacityImage.gameObject.SetActive(true);
                    CapacityImage.sprite = _characterCapacitiesSprites[_characterButtons.IndexOf(_characterSelected)];
                    _joinButton.transform.DOScale(1f, 0.5f);
                    transform.localScale = _initialTransform;
                }
            }
           
        }


        void OnSubmit() { //Valider la selection d'un personnage 

            
            if (PlayerIsJoined[PlayerIndex] == false) {
                PlayerJoined();
            }
            else if (!PlayerIsReady[PlayerIndex]) {
                if (GetComponentInParent<Button>()) {
                    //_characterSelected = GetComponentInParent<Button>();
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
                joinedText.SetActive(false);
                readyText.SetActive(true);
                
                if (_characterSelected.name == "TurtleButton")
                {
                    TurtleIsSelected = false;
                    Debug.Log("turtle deselectionnée");
                }
            }
            else if (PlayerIsJoined[PlayerIndex])
            { 
                _joinButton.transform.DOScale(1f, 0.5f);
                _joinButton.image.sprite = _characterSprites[7];
                CapacityImage.gameObject.SetActive(false);
                joinedText.SetActive(true);
                readyText.SetActive(false);
              _eventSystem.SetSelectedGameObject(_joinButton.gameObject);
              if (_characterSelected) {
                  _characterSelected.enabled = true;
              }
              PlayerIsJoined[PlayerIndex] = false;
              Debug.Log("le jooueur est parti");
              Selector.SetActive(false);
              transform.SetParent(canvas.transform);
            }
        }
        


        public void PlayerJoined() {
           
            PlayerIsJoined[PlayerIndex ] = true;
            MooveSelectorPosition();
            Debug.Log("un joueur " + PlayerIndex +" a rejoin");
            Selector.SetActive(true);
            joinedText.SetActive(false);
            readyText.SetActive(true);
            CapacityImage.gameObject.SetActive(true);
        }
        
        public void PlayerReady()
        {
            //camera.transform.DOShakePosition(1, 1);
            readyText.SetActive(false);
            PlayerIsReady[PlayerIndex] = true;
            //bool allPlayersReady = true;
            int readyCount = 0;
            Debug.Log( PlayerIsReady[PlayerIndex]);

            for (int i = 0; i < PlayerIsJoined.Length; i++) {
                if (PlayerIsJoined[i] == true) {
                    if (PlayerIsReady[i] == false) {
                      //  allPlayersReady = false;
                    }
                    else {
                        readyCount++;
                        ConfirmChoice(PlayerIndex, _characterIndex);
                      
                        if (_characterSelected.name == "TurtleButton")
                        {
                            TurtleIsSelected = true;
                            Debug.Log("turtle selectionnée");
                        }
                        Debug.Log("le joueur " + PlayerIndex +"est pret");
                        Debug.Log(readyCount+" joueurs pret");
                       
                    }
                }
            }
            if  (/*allPlayersReady == true && readyCount > _maxPlayers*/ readyCount >= _maxPlayers && TurtleIsSelected) {
                CanStart = true;
            }
            else if ( readyCount >= _maxPlayers && !TurtleIsSelected){
                CanStart = false;
                
            }
            else
            {
                CanStart = false;
            }

            if (CanStart)
            {
                circleTransition.transform.DOScale(15,1);
                Invoke("LoadSceneWarpper",1f);
            }
            
        }
        

        public void LoadSceneWarpper()
        {
            string sceneName = "Game";
            CustomSceneManager.Instance.LoadScene(sceneName);
            
        }
        
        
        
        
        
        
        
        public void ConfirmChoice(int playerIndex, int characterIndex) {

            if (!DataManager.Instance.PlayerChoice.ContainsKey(PlayerIndex))
            {
                //DataManager.Instance.PlayerCharacter.Add(playerIndex, characterIndex);
                DataManager.Instance.PlayerChoice[playerIndex] = characterIndex;


            }
            
          
        }
        public void RemoveChoice(int playerIndex) {
            DataManager.Instance.PlayerChoice.Remove(playerIndex);
        }
        
        
        
        
        
    }

}
