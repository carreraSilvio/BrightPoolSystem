using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    [System.Serializable]
    public class PoolConfig
    {
        public enum IdSource { PrefabName, Enum, Manual};
        public string id = "myStringId";
        public GameObject prefab;
        public int size = 10;
    }
}