using System.Collections;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class RoseController : FlowerController
    {
        [SerializeField] private GameObject spawnTrapPrefab;
        [SerializeField] private float respawnDelay = 5f;
        private bool _isRespawning;
        private GameObject currentTrap;

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            base.Update();
            if (isDead && !_isRespawning)
            {
                Respawn();
            }
        }

        protected override void MainCapacity()
        {
            if (sun >= CapacityCost && !IsPlanted)
            {
                if (currentTrap == null)
                {
                    currentTrap = Instantiate(spawnTrapPrefab, transform.position, transform.rotation);
                }
                else
                {
                    currentTrap.transform.position = transform.position;
                }

                OnLooseSunCapacity(CapacityCost);
            }
        }

        private void Respawn()
        {
            if (!_isRespawning)
            {
                StartCoroutine(RespawnWaiter());
            }
        }

        IEnumerator RespawnWaiter()
        {
            _isRespawning = true;
            yield return new WaitForSeconds(respawnDelay);

            if (currentTrap != null)
            {
                transform.position = currentTrap.transform.position;
                Destroy(currentTrap);
                currentTrap = null;  // Ensure reference is cleared
            }

            GetRevive();
            _isRespawning = false;
        }
    }
}