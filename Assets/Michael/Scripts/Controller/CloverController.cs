using Unity.VisualScripting;
using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class CloverController : FlowerController
    {
        [SerializeField] private GameObject[] plantedFlowers;
        
        protected override void Start()
        {
          plantedFlowers = GameObject.FindGameObjectsWithTag("Fake");
          
        }


        protected override void MainCapacity()
        {
            
            if (IsPlanted && sun >=CapacityCost && plantedFlowers.Length != 0)
            {
                Vector3 initialPosition = gameObject.transform.position;
                GameObject randomSpawnPoint = plantedFlowers[Random.Range(0, plantedFlowers.Length)];
                //Rb.isKinematic = true;
                transform.position = new Vector3(randomSpawnPoint.transform.position.x, 0,
                    randomSpawnPoint.transform.position.z);

                 randomSpawnPoint.transform.position = new Vector3(initialPosition.x,
                    randomSpawnPoint.transform.position.y, initialPosition.z);
                
                // Invoke("DisableKinematic",0.5f) ;
                 OnLooseSunCapacity(CapacityCost);
           
            }
        
        
          
        }

        private void DisableKinematic()
        {
            Rb.isKinematic = false;
        }
        
        

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        }
    }
}
