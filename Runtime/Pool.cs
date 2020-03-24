using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{

    public class Pool
    {
        public PoolEvent onPoolableAquire;
        public PoolEvent onPoolableRelease;

        private GameObject[] _entries;
        private Queue<GameObject> _available;

        private static GameObject _mainRoot;
        private GameObject _localRoot;

        public Pool(GameObject prefab, int size = 10)
        {
            var poolable = prefab.GetComponentInChildren<IPoolable>(true);
            if (poolable == null)
            {
                Debug.LogWarning($"No {nameof(IPoolable)} found in {prefab.name} prefab. Make sure one script implements it.");
                return;
            }

            _entries = new GameObject[size];
            _available = new Queue<GameObject>(size);

            FindMainRoot();
            FindLocalRoot(prefab);
            Create(prefab, size);
        }

        private void FindMainRoot()
        {
            if (_mainRoot != null) return;

            var mainRoot = GameObject.Find("PoolSystem");
            if (mainRoot == null)
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
            if (_localRoot == null)
            {
                _localRoot = new GameObject(prefab.name + "Pool");
            }
            _localRoot.transform.SetParent(_mainRoot.transform);
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

        private void HandlePoolableRelease(GameObject go)
        {
            _available.Enqueue(go);
        }

        public bool HasAvailable()
        {
            return _available.Count > 0;
        }

        public GameObject FetchAvailable()
        {
            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<IPoolable>();
            poolable.Aquire();
            return entry;
        }

        public GameObject FetchAvailable<T>(out T component) where T : MonoBehaviour 
        {
            var entry = _available.Dequeue();
            var poolable = entry.GetComponent<IPoolable>();
            poolable.Aquire();
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

        public GameObject[] Entries { get => _entries; }
        public int InUseTotal { get => _entries.Length - _available.Count; }
    }
}