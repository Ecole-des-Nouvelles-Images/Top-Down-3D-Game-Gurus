using System;
using Michael.Scripts.Controller;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class DandelionController : FlowerController
    {
        [SerializeField] private float invincibilityDuration = 5f;
        [SerializeField] private ParticleSystem pollenVfx;
        private float _invincibilityTimer;
        private bool _isBoosted;
        private bool _isColliding;

        protected override void Start()
        {
            base.Start();
            _invincibilityTimer = 0;
        }

        protected override void Update()
        {
            base.Update();

            if (isInvincible)
            {
                _invincibilityTimer += Time.deltaTime;

                if (_invincibilityTimer >= invincibilityDuration)
                {
                    isInvincible = false;
                    _invincibilityTimer = 0;
                    ResetBoost();
                }
                else if (!_isBoosted && !isDead)
                {
                    ActivateBoost();
                }
            }
            else
            {
                if (_isBoosted)
                {
                    ResetBoost();
                }
            }
        }

        protected override void PassiveCapacity()
        {
            // Implement passive capacity logic here if needed
        }

        private void ActivateBoost()
        {
            // Changer le layer mask en CanMoveThroughWalls
            gameObject.layer = LayerMask.NameToLayer("Dandelion");

            pollenVfx.Play();
            aliveModel.SetActive(false);
            moveSpeed += 300;
            _isBoosted = true;
            Debug.Log("Boost Activated");
        }

        private void ResetBoost()
        {
            gameObject.layer = LayerMask.NameToLayer("Default");

            pollenVfx.Stop();
            aliveModel.SetActive(true);
            moveSpeed = 525;
            _isBoosted = false;
            ExitObstacle();
        }

        private void ExitObstacle()
        {
            if (_isColliding)
            {
                /*
                Vector3 safePosition = FindSafePosition();
                transform.position = safePosition;
                */
                _isColliding = false;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _isColliding = true;
        }

        protected override void MainCapacity()
        {
            if (sun >= CapacityCost && !IsPlanted)
            {
                isInvincible = true;
                OnLooseSunCapacity(CapacityCost);
            }
        }
        /*
        private Vector3 FindSafePosition()
        {
            float radius = 1f; 
            float maxDistance = 10f; 

            Vector3[] directions = {
                Vector3.forward,
                Vector3.back,
                Vector3.left,
                Vector3.right,
            };

            foreach (var direction in directions)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, direction, out hit, maxDistance))
                {
                    return transform.position + direction * radius;
                }
            }

            return transform.position + Vector3.up * radius;
        }
        */
    }
}
