using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    [System.Serializable]
    public class PrefabPoolConfig
    {
        public IPoolable prefabPoolable;
        public GameObject prefab;
        public int size = 10;
    }
}