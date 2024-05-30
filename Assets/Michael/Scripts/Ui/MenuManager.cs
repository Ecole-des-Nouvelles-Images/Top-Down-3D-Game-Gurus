using System.Collections.Generic;
using DG.Tweening;
using Michael.Scripts.Character_Selection;
using Michael.Scripts.Manager;
using UnityEngine;

namespace Michael.Scripts.Ui
{
    public class MenuManager : MonoBehaviourSingleton<MenuManager>
    {
        public List<GameObject> characterPrefabs;
        public List<Transform> spawnPoints;
        [SerializeField] private GameObject finalMenuCamera;
        [SerializeField] private GameObject selectionCanvas;
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

        public void SpawnPlayerInMenu()
        {
            selectionCanvas.SetActive(false);
            finalMenuCamera.SetActive(true);
            for (int i = 0; i < CharacterSelection._maxPlayers; i++)
            {
                if (DataManager.Instance.PlayerChoice.ContainsKey(i))
                {
                    GameObject character = Instantiate(characterPrefabs[DataManager.Instance.PlayerChoice[i]],
                        spawnPoints[DataManager.Instance.PlayerChoice[i]].position,
                        Quaternion.identity, this.gameObject.transform);
                }
            }


        }
    }
}

