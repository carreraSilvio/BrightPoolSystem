using UnityEngine;
using BrightLib.RPGDatabase.Runtime;

namespace BrightLib.BrightPoolSystem.Demo
{
    public class DemoPoolSystem : MonoBehaviour
    {

        public PrefabPoolConfig[] configs;

        private PoolSystem _poolSystem;

        void Start()
        {
            _poolSystem = new PoolSystem();
            foreach(var config in configs)
            {
                _poolSystem.CreatePool(config.prefab.name, config.prefab, config.size);
            }
        }
    }

    [System.Serializable]
    public class PrefabPoolConfig
    {
        public GameObject prefab;
        public int size = 10;
    }
}