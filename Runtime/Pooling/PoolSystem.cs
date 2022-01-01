using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// A delegate for events that happen inside the pool
    /// </summary>
    /// <param name="id">The Id to the poolable object</param>
    /// <param name="poolSize">The total size of the pool</param>
    /// <param name="totalInUse">The amount of poolables currently in-use</param>
    public delegate void PoolAction(string id, int poolSize, int totalInUse);

    /// <summary>
    /// Creates and maintains all the pools available
    /// </summary>
    public sealed class PoolSystem
    {
        private readonly Dictionary<string, Pool> _pools;

        private GameObject _mainRoot;
        private static readonly string MAIN_ROOT_NAME = "Pools";
        private static readonly string LOCAL_ROOT_SUFFIX = "Pool";

        #region Singleton
        private static PoolSystem _instance;

        private static PoolSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PoolSystem();
                }

                return _instance;
            }
        }

        private PoolSystem()
        {
            _pools = new Dictionary<string, Pool>();
            CreateMainRoot();
        }
        #endregion

        #region CreatePool

        /// <inheritdoc cref="ExecuteCreatePool(string, GameObject, int)"/>
        public static void CreatePool(Enum id, GameObject prefab, int size = 10)
        {
            Instance.ExecuteCreatePool(id.ToString(), prefab, size);
        }

        /// <inheritdoc cref="ExecuteCreatePool(string, GameObject, int)"/>
        public static void CreatePool(string id, GameObject prefab, int size = 10)
        {
            Instance.ExecuteCreatePool(id, prefab, size);
        }

        /// <summary>
        /// Create a new pool and add it to list
        /// </summary>
        private void ExecuteCreatePool(string id, GameObject prefab, int size = 10)
        {
            var localRoot = GetLocalRoot(id);
            var pool = new Pool(id, prefab, size, localRoot);
            _pools.Add(id, pool);
        }

        #endregion

        #region Peek

        /// <inheritdoc cref="ExecutePeek(string)"/>
        public static GameObject[] Peek(Enum enumId)
        {
            return Instance.ExecutePeek(enumId.ToString());
        }

        /// <summary>
        /// Returns all members of a given pool
        /// </summary>
        private GameObject[] ExecutePeek(string id)
        {
            var entries = _pools[id].Entries;
            return entries;
        }

        #endregion

        #region FetchAvailable

        /// <inheritdoc cref="ExecuteFetchAvailable(string, out GameObject)"/>
        public static bool FetchAvailable(string id, out GameObject gameObject)
        {
            return Instance.ExecuteFetchAvailable(id, out gameObject);
        }

        /// <inheritdoc cref="ExecuteFetchAvailable{T}(string, out T)"/>
        public static bool FetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            return Instance.ExecuteFetchAvailable<T>(id, out component);
        }

        /// <summary>
        /// Searches for an available object and if it finds one and returns it.
        /// </summary>
        private bool ExecuteFetchAvailable(string id, out GameObject gameObject)
        {
            if (!_pools.ContainsKey(id))
            {
                gameObject = default;
                return false;
            }

            var pool = _pools[id];
            return pool.FetchAvailable(out gameObject);
        }

        /// <summary>
        /// Searches for an available object and if it finds oneand returns.
        /// </summary>
        private bool ExecuteFetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            if (!_pools.ContainsKey(id))
            {
                component = default;
                return false;
            }

            var pool = _pools[id];
            return pool.FetchAvailable(out component);
        }

        #endregion

        #region HasAvailable

        /// <inheritdoc cref="ExecuteHasAvailable(string)"/>
        public static bool HasAvailable(Enum enumId)
        {
            return Instance.ExecuteHasAvailable(enumId.ToString());
        }

        /// <inheritdoc cref="ExecuteHasAvailable(string)"/>
        public static bool HasAvailable(string id)
        {
            return Instance.ExecuteHasAvailable(id);
        }

        /// <summary>
        /// Returns true if there's an object available
        /// </summary>
        private bool ExecuteHasAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return false;

            return _pools[id].HasAvailable();
        }

        #endregion

        #region HasPool

        /// <inheritdoc cref="ExecuteHasPool(string)"/>
        public static bool HasPool(Enum enumId)
        {
            return Instance.ExecuteHasPool(enumId.ToString());
        }

        /// <inheritdoc cref="ExecuteHasPool(string)"/>
        public static bool HasPool(string id)
        {
            return Instance.ExecuteHasPool(id);
        }

        /// <summary>
        /// Returns true if there's a pool of the given id
        /// </summary>
        private bool ExecuteHasPool(string id)
        {
            return _pools.ContainsKey(id);
        }

        #endregion

        #region TotalAcquired

        /// <inheritdoc cref="ExecuteTotalAcquired(string)"/>
        public static int TotalAcquired(Enum enumId)
        {
            return Instance.ExecuteTotalAcquired(enumId.ToString());
        }

        /// <inheritdoc cref="ExecuteTotalAcquired(string)"/>
        public static int TotalAcquired(string id)
        {
            return Instance.ExecuteTotalAcquired(id);
        }

        /// <summary>
        /// Returns the total of objects in the given pool that are aquired
        /// </summary>
        private int ExecuteTotalAcquired(string id)
        {
            if (!_pools.ContainsKey(id))
            {
                Debug.LogWarning($"No {id} pool found.");
                return -1;
            }

            return _pools[id].TotalAcquired;
        }

        #endregion

        #region Event Access

        /// <inheritdoc cref="ExecuteAddListener(string, PoolEventType, PoolAction)"/>
        public static void AddListener(Enum idEnum, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteAddListener(idEnum.ToString(), evt, target);
        }

        /// <inheritdoc cref="ExecuteAddListener(string, PoolEventType, PoolAction)"/>
        public static void AddListener(string id, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteAddListener(id, evt, target);
        }

        /// <inheritdoc cref="ExecuteRemoveListener(string, PoolEventType, PoolAction)"/>
        public static void RemoveListener(Enum idEnum, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteRemoveListener(idEnum.ToString(), evt, target);
        }

        /// <inheritdoc cref="ExecuteRemoveListener(string, PoolEventType, PoolAction)"/>
        public static void RemoveListener(string id, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteRemoveListener(id, evt, target);
        }

        /// <summary>
        /// Adds a listener to <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be added</param>
        private void ExecuteAddListener(string id, PoolEventType evt, PoolAction target)
        {
            if (!_pools.ContainsKey(id))
            {
                Debug.LogWarning($"No {id} pool found.");
                return;
            }

            var pool = _pools[id];
            if (evt == PoolEventType.OnAcquire) pool.onPoolableAcquire += target;
            else pool.onPoolableRelease += target;
        }

        /// <summary>
        /// Removes a listener of <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be removed</param>
        private void ExecuteRemoveListener(string id, PoolEventType evt, PoolAction target)
        {
            if (!_pools.ContainsKey(id))
            {
                Debug.LogWarning($"No {id} pool found.");
                return;
            }

            var pool = _pools[id];
            if (evt == PoolEventType.OnAcquire) pool.onPoolableAcquire -= target;
            else pool.onPoolableRelease -= target;
        }

        #endregion

        #region ReleaseAll

        /// <inheritdoc cref="ExecuteReleaseAll"/>
        public static void ReleaseAll()
            => Instance.ExecuteReleaseAll();


        /// <summary>
        /// Go through each pool releasing all acquired objects
        /// </summary>
        private void ExecuteReleaseAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.ReleaseAll();
            }
        }

        #endregion


        #region Private Helper Methods

        private void CreateMainRoot()
        {
            if (_mainRoot == null)
            {
                _mainRoot = GameObject.Find(MAIN_ROOT_NAME);
                if (_mainRoot == null)
                {
                    _mainRoot = new GameObject(MAIN_ROOT_NAME);
                    _mainRoot.transform.SetAsLastSibling();
                }
            }
        }

        private GameObject GetLocalRoot(string id)
        {
            var localRootName = $"{id}{LOCAL_ROOT_SUFFIX}";
            var localRoot = _mainRoot.transform.Find(localRootName)?.gameObject;
            if (localRoot == null)
            {
                localRoot = new GameObject(localRootName);
                localRoot.transform.SetParent(_mainRoot.transform);
            }

            return localRoot;
        }

        #endregion

    }
}