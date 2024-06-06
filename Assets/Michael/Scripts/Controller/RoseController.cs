using System.Collections;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class RoseController : FlowerController
    {
        [SerializeField] private GameObject spawnTrap;
        [SerializeField] private float respawnDelay = 5f;

        private GameObject _currentTrap;
        private bool _isRespawning;
        
        protected override void PassiveCapacity()
        {
            isUnstoppable = true;
        }

        protected override void Start()
        {
            base.Start();
            PassiveCapacity();
        }

        protected override void Update()
        {
            base.Update();
            Respawn();
        }

        protected override void MainCapacity()
        {
            if (_currentTrap == null)
            {
                _currentTrap = Instantiate(spawnTrap, transform.position, transform.rotation);
            }
            else
            {
                _currentTrap.transform.position = transform.position;
            }
        }
        
        private void Respawn()
        {
            if (isDead && !_isRespawning)
            {
                StartCoroutine(RespawnWaiter());
            }
        }

        IEnumerator RespawnWaiter()
        {
            _isRespawning = true; 
            yield return new WaitForSeconds(respawnDelay);
            Rb.MovePosition(_currentTrap.transform.position);
            Destroy(_currentTrap);
            GetRevive();
            _isRespawning = false;  
        }
    }
}