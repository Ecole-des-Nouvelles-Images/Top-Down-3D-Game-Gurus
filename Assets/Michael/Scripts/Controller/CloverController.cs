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
            
            if (IsPlanted && sun >=CapacityCost)
            {
                Vector3 initialPosition = gameObject.transform.position;
                GameObject randomSpawnPoint = plantedFlowers[Random.Range(0, plantedFlowers.Length)];
                Rb.isKinematic = true;
                transform.position = new Vector3(randomSpawnPoint.transform.position.x, 0,
                    randomSpawnPoint.transform.position.z);

                 randomSpawnPoint.transform.position = new Vector3(initialPosition.x,
                    randomSpawnPoint.transform.position.y, initialPosition.z);
                
               Invoke("DisableKinematic",0.5f) ;
                int rngLuckCost = Random.Range(0, 4);
                if (rngLuckCost != 1)
                {
                    OnLooseSunCapacity(CapacityCost);
                }
                else
                {
                    Debug.Log("petit chanceux");
                }
           
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
