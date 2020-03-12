using UnityEngine;

public class PoolSystemInitializer : MonoBehaviour
{
    [SerializeField]
    private PrefabPoolConfig[] _configs = default;

    [SerializeField]
    private bool _createOnStart = true;

    private PoolSystem _poolSystem;

    public PoolSystem PoolSystem { get => _poolSystem; }

    // Start is called before the first frame update
    void Start()
    {
        if (_createOnStart) Create();   
    }

    private void Create()
    {
        _poolSystem = new PoolSystem();
        foreach (var config in _configs)
        {
            _poolSystem.CreatePool(config.prefab.name, config.prefab, config.size);
        }
    }
}
