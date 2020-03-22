using UnityEngine;

/// <summary>
/// Creates the pools definide by the config array
/// </summary>
public class PoolSystemInitializer : MonoBehaviour
{
    [SerializeField]
    private PrefabPoolConfig[] _configs = default;
    
    void Awake()
    {
        foreach (var config in _configs)
        {
            PoolSystem.CreatePool(config.prefab.name, config.prefab, config.size);
        }
        Destroy(gameObject);
    }
}