using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class Pool
    {
        public event PoolAction onPoolableAcquire;
        public event PoolAction onPoolableRelease;

        public GameObject[] Entries { get => _entries; }
        public int TotalAcquired { get => _entries.Length - _available.Count; }
        public GameObject LocalRoot { get => _localRoot; }
        public GameObject Prefab { get => _prefab; }


        private readonly string _id;
        private readonly GameObject _prefab;
        private readonly GameObject _localRoot;

        private readonly GameObject[] _entries;
        private readonly Queue<GameObject> _available = new Queue<GameObject>();

        public Pool(GameObject prefab, int size, GameObject localRoot)
              : this(prefab.name, prefab, size, localRoot)
        {

        }

        public Pool(string id, GameObject prefab, int size, GameObject localRoot)
        {
            var poolable = prefab.GetComponentInChildren<Poolable>(true);
            if (poolable == null)
            {
                Debug.LogWarning($"{nameof(Poolable)} script not found in [{prefab.name}] prefab. You need to have one.");
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
            for (int index = 0; index < amount; index++)
            {
                var entry = GameObject.Instantiate(prefab);
                entry.transform.SetParent(_localRoot.transform);
                entry.name = prefab.name + "_" + index;
                entry.SetActive(false);

                var poolable = entry.GetComponentInChildren<Poolable>(true);
                poolable.OnRelease += HandlePoolableRelease;
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
            poolable.Acquire();
            onPoolableAcquire?.Invoke(_id, _entries.Length, TotalAcquired, entry);

            gameObject = entry;
            return true;
        }

        public bool FetchAvailable<T>(out T component) where T : MonoBehaviour
        {
            if (!HasAvailable())
            {
                component = default;
                return false;
            }

            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<Poolable>();
            poolable.Acquire();
            onPoolableAcquire?.Invoke(_id, _entries.Length, TotalAcquired, entry);

            component = entry.GetComponent<T>();
            return true;
        }

        public void ReleaseAll()
        {
            foreach (var entry in _entries)
            {
                var poolable = entry.GetComponent<Poolable>();
                if (poolable.Acquired)
                {
                    poolable.Release();
                }
            }
        }

        public bool HasAvailable()
        {
            return _available.Count > 0;
        }

        private void HandlePoolableRelease(GameObject go)
        {
            go.transform.SetParent(_localRoot.transform);
            _available.Enqueue(go);
            onPoolableRelease?.Invoke(_id, _entries.Length, TotalAcquired, go);
        }
    }
}