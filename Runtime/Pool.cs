using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{

    public class Pool
    {
        public PoolAction onPoolableAquire;
        public PoolAction onPoolableRelease;

        private GameObject[] _entries;
        private Queue<GameObject> _available;

        private static GameObject _mainRoot;
        private GameObject _localRoot;

        private string _id;

        public Pool(GameObject prefab, int size, PoolTracker tracker)
        {
            var poolable = prefab.GetComponentInChildren<IPoolable>(true);
            if (poolable == null)
            {
                Debug.LogWarning($"No {nameof(IPoolable)} found in {prefab.name} prefab. Make sure one script implements it.");
                return;
            }

            _entries = new GameObject[size];
            _available = new Queue<GameObject>(size);

            _mainRoot = tracker.FindMainRoot(this);
            _localRoot = tracker.FindLocalRoot(this, prefab);
            _id = prefab.name;
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
        public GameObject MainRoot { get => _mainRoot; }
        public GameObject LocalRoot { get => _localRoot; }
    }
}