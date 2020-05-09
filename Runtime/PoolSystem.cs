﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// A delegate for events that happen inside the pool
    /// </summary>
    /// <param name="poolableName">The Id to the poolable object</param>
    /// <param name="poolSize">The total size of the pool</param>
    /// <param name="poolableInUse">The amount of poolables currently in-use</param>
    public delegate void PoolAction(string poolableName, int poolSize, int poolableInUse);

    /// <summary>
    /// Creates and maintains all the pools available
    /// </summary>
    public class PoolSystem
    {
        private Dictionary<string, Pool> _pools;
        private PoolTracker _poolTracker;

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
            var localRoot = _poolTracker.FindLocalRoot(prefab);
            var pool = new Pool(prefab, size, localRoot);
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
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static bool FetchAvailable(string id, out GameObject gameObject)
            => Instance.ExecuteFetchAvailable(id, out gameObject);

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
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
            pool.FetchAvailable(out gameObject);
            return true;
        }

        private bool ExecuteFetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            if (!_pools.ContainsKey(id))
            {
                component = default;
                return false;
            }

            var pool = _pools[id];
            pool.FetchAvailable(out component);
            return true;
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

        #region TotalInUse

        /// <summary>
        /// Returns the total of objects in the given pool that are "in use"
        /// </summary>
        public static int TotalInUse(Enum enumId)
         => TotalInUse(enumId.ToString());

        /// <summary>
        /// Returns the total of objects in the given pool that are "in use"
        /// </summary>
        public static int TotalInUse(string id)
        {
            if (!Instance._pools.ContainsKey(id)) return 0;

            var pool = Instance._pools[id];
            return pool.TotalInUse;
        }

        #endregion

        #region Event Access

        /// <summary>
        /// Adds a listener to <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be added</param>
        public static void AddListener(string id, PoolEvent evt, PoolAction target)
        {
            Instance.ExecuteAddListener(id, evt, target);
        }

        /// <summary>
        /// Removes a listener of <paramref name="id"/>'s <paramref name="evt"/>
        /// </summary>
        /// <param name="id">The poolable ID</param>
        /// <param name="evt">The event type</param>
        /// <param name="target">The callback method to be removed</param>
        public static void RemoveListener(string id, PoolEvent evt, PoolAction target)
        {
            Instance.ExecuteRemoveListener(id, evt, target);
        }

        private void ExecuteAddListener(string id, PoolEvent evt, PoolAction target)
        {
            if (!_pools.ContainsKey(id))
            {
                Debug.LogWarning($"No {id} pool found.");
                return;
            }

            var pool = _pools[id];
            if (evt == PoolEvent.OnAquire) pool.onPoolableAquire += target;
            else pool.onPoolableRelease += target;
        }

        private void ExecuteRemoveListener(string id, PoolEvent evt, PoolAction target)
        {
            if (!_pools.ContainsKey(id))
            {
                Debug.LogWarning($"No {id} pool found.");
                return;
            }

            var pool = _pools[id];
            if (evt == PoolEvent.OnAquire) pool.onPoolableAquire -= target;
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