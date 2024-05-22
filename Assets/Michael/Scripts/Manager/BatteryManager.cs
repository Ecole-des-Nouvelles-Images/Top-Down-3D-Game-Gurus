using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class BatteryManager : MonoBehaviourSingleton<BatteryManager>
    {
        public float CurrentBatteryTime;
        [SerializeField] private float _maxBatteryTime;
        
  
    
        void Start() {
            CurrentBatteryTime = _maxBatteryTime;
        
        }
        
        void Update() {
            
            CurrentBatteryTime -= Time.deltaTime;

            if (CurrentBatteryTime <= 0 && !GameManager.Instance.TurtleIsDead){
                
                // animation 
                //particule 
                GameManager.Instance.TurtleIsDead = true;
            }

            if (CurrentBatteryTime > _maxBatteryTime) {
                CurrentBatteryTime = _maxBatteryTime;
            }
            if (CurrentBatteryTime < _maxBatteryTime) {
                CurrentBatteryTime = _maxBatteryTime;
            }
        }


       public void BatteryCost(float capacityCost)
        {
            CurrentBatteryTime -= capacityCost;
        }
        
        
        
        
    }
}
