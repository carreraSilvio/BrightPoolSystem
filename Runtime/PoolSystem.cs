using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates and maintains all the pools available
/// </summary>
public class PoolSystem
{
    private Dictionary<string, PrefabPool> _prefabPools;

    public PoolSystem()
    {
        _prefabPools = new Dictionary<string, PrefabPool>();
    }

    public void CreatePool(string id, GameObject prefab, int size = 10)
    {
        var pool = new PrefabPool(prefab, size);
        _prefabPools.Add(id, pool);
    }

    public GameObject FetchAvailable(string id)
    {
        if (!_prefabPools.ContainsKey(id)) return null;

        var pool = _prefabPools[id];
        return pool.FetchAvailable();
    }

    public GameObject FetchAvailable(string id, out IPrefabPoolable poolable)
    {
        if (!_prefabPools.ContainsKey(id))
        {
            poolable = null;
            return null;
        }

        var pool = _prefabPools[id];
        return pool.FetchAvailable(out poolable);
    }
}
