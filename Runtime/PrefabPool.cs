using System.Collections.Generic;
using UnityEngine;

public class PrefabPool
{
    private List<GameObject> _entries;
    private Queue<GameObject> _available;

    private static GameObject _mainRoot;
    private GameObject _localRoot;

    public PrefabPool(GameObject prefab, int size = 10)
    {
        _entries = new List<GameObject>(size);
        _available = new Queue<GameObject>(size);

        FindMainRoot();
        FindLocalRoot(prefab);
        Create(prefab, size);
    }

    private void FindMainRoot()
    {
        if (_mainRoot != null) return;

        var mainRoot = GameObject.Find("PoolSystem");
        if(mainRoot == null)
        {
            mainRoot = new GameObject("PoolSystem");
            mainRoot.transform.SetAsLastSibling();
        }
        _mainRoot = mainRoot;
    }

    private void FindLocalRoot(GameObject prefab)
    {
        if (_localRoot != null) return;

        _localRoot = GameObject.Find(prefab.name + "Pool");
        if(_localRoot == null)
        {
            _localRoot = new GameObject(prefab.name + "Pool");
        }
        _localRoot.transform.SetParent(_mainRoot.transform);
    }

    private void Create(GameObject prefab, int amount = 10)
    {
        while (amount > 0)
        {
            var go = GameObject.Instantiate(prefab);
            go.transform.SetParent(_localRoot.transform);
            go.name = prefab.name + _entries.Count;
            go.SetActive(false);

            var poolable = go.GetComponentInChildren<IPrefabPoolable>(true);
            if(poolable == null)
            {
                Debug.LogWarning("No IPrefabPoolable found. Make sure your prefab has one script that implements IPrefabPoolable");
                return;
            }
            poolable.onRelease += HandleEntryRelease;
            _entries.Add(go);
            _available.Enqueue(go);

            amount--;
        }
    }

    private void HandleEntryRelease(GameObject go)
    {
        _available.Enqueue(go);
    }

    public bool HasAvailable()
    {
        return (_available.Count > 0);
    }

    public IPrefabPoolable FetchAvailable()
    {
        var entry = _available.Dequeue();
        var poolable = entry.GetComponent<IPrefabPoolable>();
        poolable.Aquire();
        return poolable;
    }

    public void ReleaseAll()
    {
        foreach (var entry in _entries)
        {
            var poolable = entry.GetComponent<IPrefabPoolable>();
            poolable.Release();
        }
    }
}
