using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Creates the pools definide by the config array
    /// </summary>
    public class PoolSystemInitializer : MonoBehaviour
    {
        [SerializeField]
        private PoolConfig[] _configs = default;

        void Awake()
        {
            foreach (var config in _configs)
            {
                PoolSystem.CreatePool(config.id, config.prefab, config.size);
            }
            Destroy(gameObject);
        }
    }
}