using UnityEngine;

namespace Michael.Scripts.Ui
{
    public class UiLookAtCamera : MonoBehaviour 
    {
    
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject canvas;
        //[SerializeField] private Image imagetoFollow;
        void Start() {
            camera = Camera.main;
        }
        
        void Update() {
            canvas.transform.LookAt(2* canvas.transform.position -camera.transform.position);
        }
    }
 
}
