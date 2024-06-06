using System.Collections;
using UnityEngine;

namespace Noah.Scripts
{
    public class RoseController : FlowerController
    {
        [SerializeField] private GameObject spawnTrap;
        [SerializeField] private float respawnDelay = 5f;
        
        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Update()
        {
            PassiveCapacity();
            base.Update();
            Respawn();
        }

        protected override void MainCapacity()
        {
            if (spawnTrap != null)
            {
                Instantiate(spawnTrap, transform.position, transform.rotation);
            }

            else
            {
                spawnTrap.transform.position = transform.position;
            }
        }
        
        

        private void Respawn()
        {
            if (isDead)
            {
                StartCoroutine(RespawnWaiter());
            }
        }

        IEnumerator RespawnWaiter()
        {
            yield return new WaitForSeconds(respawnDelay);
            transform.position = spawnTrap.transform.position;
            GetRevive();
        }
    }
}
