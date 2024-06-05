
using UnityEngine;
using UnityEngine.UI;

namespace Michael.Scripts.Manager
{
    public class BatteryManager : MonoBehaviourSingleton<BatteryManager>
    {
        public float CurrentBatteryTime;
        [SerializeField] private float _maxBatteryTime;
        [SerializeField] private float _easeSilerRate =1;
        [SerializeField] private Slider _batteryBar;
        [SerializeField] private Slider _easeBatteryBar;
        private float _lerpSpeed = 0.05f;
        private float _delayBatteryTime;
    
        void Start()
        {
            CurrentBatteryTime = _maxBatteryTime;
            _delayBatteryTime = _maxBatteryTime;
        }
        
        void Update() {
            
           CurrentBatteryTime -= TimeManager.Instance.deltaTime;
           _delayBatteryTime -= TimeManager.Instance.deltaTime *_easeSilerRate;
           _delayBatteryTime = Mathf.Clamp(_delayBatteryTime,CurrentBatteryTime, _maxBatteryTime);
           
           _batteryBar.value = CurrentBatteryTime / _maxBatteryTime;
            _easeBatteryBar.value = _delayBatteryTime / _maxBatteryTime;
            
            if (CurrentBatteryTime <= 0 && !GameManager.Instance.TurtleIsDead){
               
                GameManager.Instance.TurtleIsDead = true;
            }

            if (CurrentBatteryTime > _maxBatteryTime)
            {
                CurrentBatteryTime = _maxBatteryTime;
            }
        }
        
       public void BatteryCost(float capacityCost)
        {
            CurrentBatteryTime -= capacityCost;
        }
        
        
    }
}
