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
        private readonly PoolTracker _poolTracker;

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
            _poolTracker = new PoolTracker();
            _poolTracker.FindMainRoot();
        }

        #region CreatePool

        /// <summary>
        /// Create a new pool and add it to list
        /// </summary>
        public static void CreatePool(string id, GameObject prefab, int size = 10)
            => Instance.ExecuteCreatePool(id, prefab, size);

        private void ExecuteCreatePool(string id, GameObject prefab, int size = 10)
        {
            var localRoot = _poolTracker.FindLocalRoot(id);
            var pool = new Pool(id, prefab, size, localRoot);
            _pools.Add(id, pool);
        }

        #endregion

        #region Peek

        /// <summary>
        /// Returns all members of a given pool
        /// </summary>
        public static GameObject[] Peek(Enum enumId)
            => Instance.ExecutePeek(enumId.ToString());

        private GameObject[] ExecutePeek(string id)
        {
            var entries = _pools[id].Entries;
            return entries;
        }

        #endregion

        #region FetchAvailable

        /// <summary>
        /// Searches for an available object and if it finds one and returns it.
        /// </summary>
        public static bool FetchAvailable(string id, out GameObject gameObject)
            => Instance.ExecuteFetchAvailable(id, out gameObject);

        /// <summary>
        /// Searches for an available object and if it finds oneand returns.
        /// </summary>
        public static bool FetchAvailable<T>(string id, out T component) where T : MonoBehaviour
            => Instance.ExecuteFetchAvailable<T>(id, out component);

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

        /// <summary>
        /// Returns true if there's an object available
        /// </summary>
        public static bool HasAvailable(Enum enumId)
            => HasAvailable(enumId.ToString());

        /// <summary>
        /// Returns true if there's an object available
        /// </summary>
        public static bool HasAvailable(string id)
            => Instance.ExecuteHasAvailable(id);
        private bool ExecuteHasAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return false;

            return _pools[id].HasAvailable();
        }

        #endregion

        #region HasPool

        /// <summary>
        /// Returns true if there's a pool of the given id
        /// </summary>
        public static bool HasPool(Enum enumId)
            => HasPool(enumId.ToString());

        /// <summary>
        /// Returns true if there's a pool of the given id
        /// </summary>
        public static bool HasPool(string id)
            => Instance.ExecuteHasPool(id);

        private bool ExecuteHasPool(string id)
        {
            return _pools.ContainsKey(id);
        }

        #endregion

        #region TotalAcquired

        /// <inheritdoc cref="ExecuteTotalAcquired(string)"/>
        public static int TotalAcquired(Enum enumId)
            => Instance.ExecuteTotalAcquired(enumId.ToString());

        /// <inheritdoc cref="ExecuteTotalAcquired(string)"/>
        public static int TotalAcquired(string id)
            => Instance.ExecuteTotalAcquired(id);

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

        /// <summary>
        /// Adds a listener to <paramref name="idEnum"/>'s <paramref name="evt"/>
        /// </summary>
        public static void AddListener(Enum idEnum, PoolEventType evt, PoolAction target)
            => Instance.ExecuteAddListener(idEnum.ToString(), evt, target);


        /// <summary>
        /// Adds a listener to <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be added</param>
        public static void AddListener(string id, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteAddListener(id, evt, target);
        }

        /// <summary>
        /// Removes a listener of <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        public static void RemoveListener(Enum idEnum, PoolEventType evt, PoolAction target)
            => Instance.ExecuteRemoveListener(idEnum.ToString(), evt, target);

        /// <summary>
        /// Removes a listener of <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be removed</param>
        public static void RemoveListener(string id, PoolEventType evt, PoolAction target)
        {
            Instance.ExecuteRemoveListener(id, evt, target);
        }

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

        /// <summary>
        /// Go through each pool releasing all acquired objects
        /// </summary>
        public static void ReleaseAll()
            => Instance.ExecuteReleaseAll();

        private void ExecuteReleaseAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.ReleaseAll();
            }
        }

        #endregion
    }
}