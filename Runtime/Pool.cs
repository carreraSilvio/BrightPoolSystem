using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class Pool
    {
        public PoolAction onPoolableAquire;
        public PoolAction onPoolableRelease;

        private GameObject _prefab;
        private GameObject[] _entries;
        private Queue<GameObject> _available;

        private GameObject _localRoot;

        private readonly string _id;

        public Pool(GameObject prefab, int size, GameObject localRoot)
              : this(prefab.name, prefab, size, localRoot)
        { 
            
        }

        public Pool(string id, GameObject prefab, int size, GameObject localRoot)
        {
            var poolable = prefab.GetComponentInChildren<IPoolable>(true);
            if (poolable == null)
            {
                Debug.LogWarning($"No {nameof(IPoolable)} found in {prefab.name} prefab. Make sure one script implements it.");
                return;
            }

            _prefab = prefab;
            _entries = new GameObject[size];
            _available = new Queue<GameObject>(size);

            _localRoot = localRoot;
            _id = id;
            Create(prefab, size);
        }

        private void Create(GameObject prefab, int amount = 10)
        {
            int index = 0;
            while (amount > 0)
            {
                var go = Object.Instantiate(prefab);
                go.transform.SetParent(_localRoot.transform);
                go.name = prefab.name + index;
                go.SetActive(false);

                var poolable = go.GetComponentInChildren<IPoolable>(true);
                poolable.onRelease += HandlePoolableRelease;
                _entries[index++] = go;
                _available.Enqueue(go);

                amount--;
            }
        }

        public GameObject FetchAvailable()
        {
            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<IPoolable>();
            poolable.Aquire();
            onPoolableAquire?.Invoke(_id, _entries.Length, InUseTotal);

            return entry;
        }

        public GameObject FetchAvailable<T>(out T component) where T : MonoBehaviour 
        {
            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<IPoolable>();
            poolable.Aquire();
            onPoolableAquire?.Invoke(_id, _entries.Length, InUseTotal);

            component = entry.GetComponent<T>();
            return entry;
        }

        public void ReleaseAll()
        {
            foreach (var entry in _entries)
            {
                var poolable = entry.GetComponent<IPoolable>();
                poolable.Release();
            }
        }

        public bool HasAvailable()
        {
            return _available.Count > 0;
        }

        private void HandlePoolableRelease(GameObject go)
        {
            _available.Enqueue(go);
            onPoolableRelease?.Invoke(_id, _entries.Length, InUseTotal);
        }

        public GameObject[] Entries { get => _entries; }
        public int InUseTotal { get => _entries.Length - _available.Count; }
        public GameObject LocalRoot { get => _localRoot; }
        public GameObject Prefab { get => _prefab; set => _prefab = value; }
    }
}