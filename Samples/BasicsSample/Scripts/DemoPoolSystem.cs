using UnityEngine;

namespace BrightLib.BrightPoolSystem.Demo
{
    public class DemoPoolSystem : MonoBehaviour
    {
        public PrefabPoolConfig[] configs;

        private PoolSystem _poolSystem;

        void Start()
        {
            _poolSystem = new PoolSystem();
            foreach (var config in configs)
            {
                _poolSystem.CreatePool(config.prefab.name, config.prefab, config.size);
            }

            var obj = _poolSystem.FetchAvailable(configs[0].prefab.name);
            Debug.Log($"Fetched object. His name is {obj.name}");
        }
    }
}