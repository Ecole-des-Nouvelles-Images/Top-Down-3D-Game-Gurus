using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
        private void Start() {
            CanStart = false;
            PlayerIsReady = new bool[5] {false, false, false, false,false};
            PlayerIsJoined = new bool[5] {false, false, false, false,false};
        }


        void OnSubmit() {
            if (GetComponentInParent<Button>()) {
                
                _characterSelected = GetComponentInParent<Button>();
                Debug.Log("un bouton est sleectionné");
                _characterIndex = _characterButtons.IndexOf(_characterSelected);
                Debug.Log(_characterIndex);


                if (_characterSelected.GetComponentInChildren<HorizontalLayoutGroup>().transform.childCount >= 2)
                {
                    Debug.Log("il a plus de 1 joueur sur ce personnage");
                }
            }
        }


        public void PlayerJoined() {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;
            PlayerIsJoined[PlayerIndex ] = true;
        }
        
        
        
        
        
    }

}
