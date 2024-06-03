using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParticules : MonoBehaviour
{
    [SerializeField] private GameObject _crashParticules;
    [SerializeField] private ParticleSystem _dirtParticules;
    private Animator _animator;

    private void Start()
    {

        _animator = GetComponent<Animator>();

    }

    public void ShowCrashParticules() {
        if (_crashParticules) {
            _crashParticules.SetActive(true);  
        }
    }
    
    public void ShowDirtParticules() {
        if (_dirtParticules) {
            _dirtParticules.Play();
        }
    }
    
    public void StopAnimation() {
        _animator.SetBool("Attack",false);
    }
    
    
    
}
