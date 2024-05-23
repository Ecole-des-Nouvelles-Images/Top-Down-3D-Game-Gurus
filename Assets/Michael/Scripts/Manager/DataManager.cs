using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class DataManager :  MonoBehaviourSingleton<DataManager>
    {
     //   public Dictionary<int, int> PlayerCharacter = new Dictionary<int, int>();

     public List<GameObject> PlayerChoice = new List<GameObject>(4);
     public List<GameObject> CharacterPrefabs = new List<GameObject>(6);
     public GameObject loadingScreen;
    }
}
