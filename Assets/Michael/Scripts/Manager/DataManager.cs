using System.Collections.Generic;

namespace Michael.Scripts.Manager
{
    public class DataManager :  MonoBehaviourSingleton<DataManager>
    {
        public Dictionary<int, int> PlayerCharacter = new Dictionary<int, int>();
    }
}
