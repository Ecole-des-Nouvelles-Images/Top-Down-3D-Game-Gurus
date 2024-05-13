using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
    
        public void LoadScene() {
            CustomSceneManager.Instance.LoadScene(sceneToLoad);
        }
    }
}
