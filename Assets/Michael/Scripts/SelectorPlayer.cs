using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectorPlayer : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private EventSystem _eventSystem;

    void OnNavigate() {
        if (_eventSystem.currentSelectedGameObject) {
          transform.SetParent( _eventSystem.currentSelectedGameObject
              .GetComponentInChildren<HorizontalLayoutGroup>().transform);
        }
    }
    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
    }
    
}
