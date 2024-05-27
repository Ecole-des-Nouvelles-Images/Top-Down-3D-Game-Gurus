using System.Collections.Generic;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class DataManager :  MonoBehaviourSingleton<DataManager>
    {
       public Dictionary<int, int> PlayerChoice = new Dictionary<int, int>();
       public GameObject loadingScreen;
    }
}
