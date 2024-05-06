using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael.Scripts
{
    public class CharacterSelection : MonoBehaviour
    {
        public static bool[] PlayerIsReady;
        public static bool[] PlayerIsJoined;
        public static bool CanStart;
        public int PlayerIndex ;
        public int _maxPlayers ;
       
        
        
        private void Start()
        {
            
            
            CanStart = false;
            PlayerIsReady = new bool[5] {false, false, false, false,false};
            PlayerIsJoined = new bool[5] {false, false, false, false,false};
        }
        
        public void PlayerJoined()
        
        {
            PlayerIndex = GetComponent<PlayerInput>().playerIndex;
            PlayerIsJoined[PlayerIndex ] = true;

        }
        
        
        
        
        
        
        
        
        
        
        
    }

}
