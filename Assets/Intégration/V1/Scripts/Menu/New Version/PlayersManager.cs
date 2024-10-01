using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public int playerIndex;
    void Start()
    {
        Gamepad[ ] gamepads = Gamepad.all.ToArray();
        InputDevice device = gamepads[playerIndex];
        PlayerInput playerInput = playerInputManager.JoinPlayer(playerIndex, -1,"Gamepad", device);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
