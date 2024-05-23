
using UnityEngine;
using UnityEngine.UI;

namespace Michael.Scripts.Manager
{
    public class BatteryManager : MonoBehaviourSingleton<BatteryManager>
    {
        public float CurrentBatteryTime;
        [SerializeField] private float _maxBatteryTime;
        [SerializeField] private Image _batteryBar;
  
    
        void Start() {
            CurrentBatteryTime = _maxBatteryTime;
            _batteryBar.fillAmount = 1;
        }
        
        void Update() {
            
            CurrentBatteryTime -= Time.deltaTime;
            int intCurrentBattery = (int)CurrentBatteryTime;
            _batteryBar.fillAmount = intCurrentBattery / _maxBatteryTime;

            if (CurrentBatteryTime <= 0 && !GameManager.Instance.TurtleIsDead){
                
                // animation 
                //particule 
                GameManager.Instance.TurtleIsDead = true;
            }

            if (CurrentBatteryTime > _maxBatteryTime) {
                CurrentBatteryTime = _maxBatteryTime;
            }
            if (CurrentBatteryTime <= 0)
            { 
                CurrentBatteryTime = 0;
                if (!GameManager.Instance.FlowersAreDead) {
                    GameManager.Instance.TurtleIsDead = true;
                }
            }
        }


       public void BatteryCost(float capacityCost)
        {
            CurrentBatteryTime -= capacityCost;
        }
        
        
    }
}
