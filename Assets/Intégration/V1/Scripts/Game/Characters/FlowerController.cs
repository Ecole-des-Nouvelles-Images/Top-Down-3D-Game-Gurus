using System;
using DG.Tweening;
using Intégration.V1.Scripts.UI;
using Michael.Scripts.Manager;
using Michael.Scripts.Ui;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Intégration.V1.Scripts.Game.Characters
{
    public abstract class FlowerController : CharacterController
    {
        [SerializeField] private AudioSource minesound;
        [SerializeField] private AudioSource revivesound;
        [SerializeField] private AudioSource capacitysound;
        public int characterIndex;
        public static Action OnSunCollected;
        public int sun = 0;
        public int maxSun = 3;
        public int CapacityCost = 2;
        public bool canReanimate;
        public FlowerController deadFlowerController;
        public bool IsPlanted = false;
        public bool isInvincible = false;
        public bool isUnhittable = false;
        public bool isUnstoppable = false;

        public bool IsStunned;
        public bool isDead;
        public static bool FlowersWin;
        public bool isCharging;
        public float reanimateTimer = 0;
        [SerializeField] private float reanimateDuration = 1;
        [SerializeField] protected GameObject deadModel;
        [SerializeField] protected GameObject aliveModel;
        [SerializeField] protected Collider aliveModelCollider;
        [SerializeField] private float magnetudeToStun = 22f;
        [SerializeField] private float stunDuration = 3f;
        [SerializeField] private float stunTimer = 0;
        [SerializeField] private ParticleSystem explosionParticleSystem;
        [SerializeField] private ParticleSystem stunParticleSystem;
        [SerializeField] private float plantingCooldown = 0.7f;
        private float currentPlantingCooldown = 0f;
        [SerializeField] private Image reviveChargingIcon;
        [SerializeField] private GameObject deadArrowUI;
        [SerializeField] private VisualEffect ReviveVFX;
        [SerializeField] private GameObject dirt;
        [SerializeField] private ParticleSystem fireWorksParticules;
        public VisualEffect GatherEnergy;


        protected virtual void Start()
        {
            StartAnimation();
        }

        private void StartAnimation()
        {
            if (deadArrowUI)
            {
                deadArrowUI.transform.DOLocalMoveY(0.25f, 1)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        deadArrowUI.transform.DOLocalMoveY(-0.25f, 1)
                            .SetEase(Ease.InOutSine)
                            .OnComplete(() => { StartAnimation(); });
                    });
            }
        }

        protected override void FixedUpdate()
        {
            if (!IsStunned)
            {
                Move();
            }
        }

        protected override void Update()
        {
            if (GameManager.Instance.TurtleIsDead && !FlowersWin)
            {
                _animator.SetTrigger("Victory");
                _animator.SetInteger("DanceIndex", Random.Range(0, 3));
                //  fireWorksParticules.Play();
                FlowersWin = true;
            }

            _animator.SetFloat("Velocity", Rb.velocity.magnitude);

            if (sun < 0)
            {
                sun = 0;
            }

            if (sun > maxSun)
            {
                sun = maxSun;
            }

            if (deadFlowerController)
            {
                if (isCharging)
                {
                    deadFlowerController.reviveChargingIcon.fillAmount = 0;
                    reanimateTimer += Time.deltaTime;
                    deadFlowerController.reviveChargingIcon.fillAmount = reanimateTimer / reanimateDuration;

                    if (reanimateTimer >= reanimateDuration + 0.1f)
                    {
                        ThirdCapacity();
                        isCharging = false;
                        reanimateTimer = 0;
                    }
                }
                else
                {
                    DOTween.To(() => deadFlowerController.reviveChargingIcon.fillAmount,
                        value => deadFlowerController.reviveChargingIcon.fillAmount = value, 0f, 0.3f);
                }
            }


            if (IsStunned)
            {
                stunTimer += Time.deltaTime;
                if (stunTimer >= stunDuration)
                {
                    stunParticleSystem.gameObject.SetActive(false);
                    stunParticleSystem.Clear();
                    IsStunned = false;
                    stunTimer = 0;
                    _animator.SetBool("IsDizzy", false);
                }
            }

            if (currentPlantingCooldown > 0)
            {
                currentPlantingCooldown -= Time.deltaTime;
            }
        }

        public override void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.performed && !IsStunned && !PauseControlller.IsPaused)
            {
                MainCapacity();
                capacitysound.Play();
            }
        }


        public override void OnSecondaryCapacity(InputAction.CallbackContext context)
        {
            if (context.started && !IsStunned && !PauseControlller.IsPaused)
            {
                if (!IsStunned && currentPlantingCooldown <= 0)
                {
                    if (!IsPlanted)
                    {
                        SecondaryCapacity();
                    }
                    else
                    {
                        GetUnplanted();
                    }
                }
            }
        }

        protected override void SecondaryCapacity()
        {
            // SE PLANTER DANS LE SOL 

            GetPlanted();
            currentPlantingCooldown = plantingCooldown;
        }

        private void GetUnplanted()
        {
            if (IsPlanted)
            {
                Instantiate(dirt, transform.localPosition, quaternion.identity);
                IsPlanted = false;
                Rb.isKinematic = false;
                _animator.SetBool("isPlanted", IsPlanted);
                aliveModelCollider.enabled = true;
                currentPlantingCooldown = plantingCooldown;
            }
        }

        public override void OnThirdCapacity(InputAction.CallbackContext context)
        {
            // REANIMATION

            if (canReanimate && sun == maxSun && !IsStunned && !PauseControlller.IsPaused)
            {
                if (context.started)
                {
                    isCharging = true;
                }
                else if (context.canceled)
                {
                    isCharging = false;
                    reanimateTimer = 0;
                }
            }
        }

        protected override void ThirdCapacity() // revive ally 
        {
            Debug.Log("revive");
            sun = -maxSun;
            deadFlowerController.GetRevive();
            canReanimate = false;
        }

        protected abstract void PassiveCapacity();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Turtle Collider"))
            {
                TakeHit();
            }

            if (other.CompareTag("TurtleTrap"))
            {
                if (!isDead)
                {
                    GameManager.Instance.TurtleTrap.Remove(other.gameObject);
                    Destroy(other.gameObject);
                    GetStunned();
                    minesound.Play();
                }
            }

            if (other.CompareTag("Seed"))
            {
                canReanimate = true;
                deadFlowerController = other.GetComponentInParent<FlowerController>();
                deadFlowerController.reviveChargingIcon.gameObject.SetActive(true);
                deadFlowerController.deadArrowUI.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Turtle") && !isDead)
            {
                Rigidbody turtleRb = other.gameObject.GetComponent<Rigidbody>();
                if (turtleRb.velocity.magnitude > magnetudeToStun)
                {
                    GetStunned();
                }
            }

            if (other.CompareTag("Shield"))
            {
                isInvincible = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Seed"))
            {
                canReanimate = false;
                isCharging = false;
                reanimateTimer = 0;
                if (deadFlowerController)
                {
                    deadFlowerController.reviveChargingIcon.gameObject.SetActive(false);
                    deadFlowerController.deadArrowUI.SetActive(true);
                    deadFlowerController = null;
                }

                if (other.CompareTag("Shield"))
                {
                    isInvincible = false;
                }
            }
        }


        private void GetPlanted()
        {
            IsPlanted = true;
            Rb.isKinematic = true;
            _animator.SetBool("isPlanted", IsPlanted);
            aliveModelCollider.enabled = false;
        }

        [ContextMenu("GetStunned")]
        private void GetStunned()
        {
            explosionParticleSystem.Play();

            if (!isInvincible && !isUnstoppable && !isUnhittable)
            {
                stunParticleSystem.gameObject.SetActive(true);
                stunParticleSystem.Play();
                GetUnplanted();
                _animator.SetBool("IsDizzy", true);
                IsStunned = true;
            }
        }


        [ContextMenu("TakeHit")]
        private void TakeHit()
        {
            if (!isInvincible && !isUnhittable)
            {
                aliveModelCollider.enabled = false;
                aliveModel.SetActive(false);
                deadModel.SetActive(true);
                GetComponent<PlayerInput>().enabled = false;
                isDead = true;
                sun = 0;
                GameManager.Instance.FlowersAlive.Remove(this.gameObject);
                deadArrowUI.SetActive(true);
                GameManager.Instance.Winverification();
            }
        }

        [ContextMenu("GetRevive")]
        public void GetRevive()
        {
            if (!isDead) return;

            aliveModelCollider.enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            isDead = false;
            aliveModel.SetActive(true);
            deadModel.SetActive(false);
            GameManager.Instance.FlowersAlive.Add(this.gameObject);
            ReviveVFX.Play();
            revivesound.Play();
        }


        public void OnLooseSunCapacity(int capacityCost)
        {
            sun -= capacityCost;
        }
    }
}