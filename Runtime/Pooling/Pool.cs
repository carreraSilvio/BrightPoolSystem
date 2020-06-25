using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class Pool
    {
        public event PoolAction onPoolableAquire;
        public event PoolAction onPoolableRelease;

        private GameObject _localRoot;

        private readonly string _id;
        private GameObject _prefab;

        private GameObject[] _entries;
        private Queue<GameObject> _available;

        public Pool(GameObject prefab, int size, GameObject localRoot)
              : this(prefab.name, prefab, size, localRoot)
        { 
            
        }

        public Pool(string id, GameObject prefab, int size, GameObject localRoot)
        {
            var poolable = prefab.GetComponentInChildren<Poolable>(true);
            if (poolable == null)
            {
                Debug.LogWarning($"No {nameof(Poolable)} found in {prefab.name} prefab. Make sure one script extends from it.");
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
            for(int index = 0; index < amount; index++)
            {
                var entry = Object.Instantiate(prefab);
                entry.transform.SetParent(_localRoot.transform);
                entry.name = prefab.name + index;
                entry.SetActive(false);

                var poolable = entry.GetComponentInChildren<Poolable>(true);
                poolable.onRelease += HandlePoolableRelease;
                _entries[index] = entry;
                _available.Enqueue(entry);
            }
        }

        public bool FetchAvailable(out GameObject gameObject)
        {
            if (!HasAvailable())
            {
                gameObject = default;
                return false;
            }

            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<Poolable>();
            poolable.Aquire();
            onPoolableAquire?.Invoke(_id, _entries.Length, TotalAquired);

            gameObject = entry;
            return true;
        }

        public bool FetchAvailable<T>(out T component) where T : MonoBehaviour 
        {
            if(!HasAvailable())
            {
                component = default;
                return false;
            }

            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<Poolable>();
            poolable.Aquire();
            onPoolableAquire?.Invoke(_id, _entries.Length, TotalAquired);

            component = entry.GetComponent<T>();
            return true;
        }

        public void ReleaseAll()
        {
            foreach (var entry in _entries)
            {
                var poolable = entry.GetComponent<Poolable>();
                if(poolable.Aquired) poolable.Release();
            }
        }

        public bool HasAvailable()
        {
            return _available.Count > 0;
        }

        private void HandlePoolableRelease(GameObject go)
        {
            _available.Enqueue(go);
            onPoolableRelease?.Invoke(_id, _entries.Length, TotalAquired);
        }

        public GameObject[] Entries { get => _entries; }
        public int TotalAquired { get => _entries.Length - _available.Count; }
        public GameObject LocalRoot { get => _localRoot; }
        public GameObject Prefab { get => _prefab;}
    }
}