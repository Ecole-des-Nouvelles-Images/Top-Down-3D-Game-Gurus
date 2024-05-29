using DG.Tweening;
using UnityEngine;

namespace Michael.Scripts.Ui
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuCanvas;

    

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void FadeInPanel(GameObject panel)
        {
            panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }
        
        public void FadeOutPanel(GameObject panel)
        {
            panel.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        }
    
    
    }
}
