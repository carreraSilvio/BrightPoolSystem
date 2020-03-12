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
}
