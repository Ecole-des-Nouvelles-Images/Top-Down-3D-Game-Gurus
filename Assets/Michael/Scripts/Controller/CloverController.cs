using UnityEngine;

namespace Michael.Scripts.Controller
{
    public class CloverController : FlowerController
    {
        [SerializeField] private GameObject[] plantedFlowers;
        [SerializeField] private Transform initialPosition;
        protected override void MainCapacity()
        {
            
            if (IsPlanted)
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
                    OnLooseSunCapacity(2);
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
