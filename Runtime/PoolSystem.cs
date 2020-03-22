using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Creates and maintains all the pools available
    /// </summary>
    public class PoolSystem
    {
        private Dictionary<string, PrefabPool> _pools;

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
            _pools = new Dictionary<string, PrefabPool>();
        }

        public static void CreatePool(string id, GameObject prefab, int size = 10)
        {
            Instance.ExecuteCreatePool(id, prefab, size);
        }

        /// <summary>
        /// Returns all members of a given pool
        /// </summary>
        public static GameObject[] Peek(Enum enumId)
        {
            return Instance.ExecutePeek(enumId.ToString());
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static GameObject FetchAvailable(Enum enumId)
        {
            return FetchAvailable(enumId.ToString());
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static GameObject FetchAvailable(string id)
        {
            return Instance.ExecuteFetchAvailable(id);
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static GameObject FetchAvailable<T>(Enum enumId, out T poolable) where T : IPrefabPoolable
        {
            return FetchAvailable(enumId.ToString(), out poolable);
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static GameObject FetchAvailable<T>(string id, out T poolable) where T : IPrefabPoolable
        {
            return Instance.ExecuteFetchAvailable(id, out poolable);
        }

        /// <summary>
        /// Returns true if there's a object available
        /// </summary>
        public static bool HasAvailable(Enum enumId)
        {
            return HasAvailable(enumId.ToString());
        }

        /// <summary>
        /// Returns true if there's a object available
        /// </summary>
        public static bool HasAvailable(string id)
        {
            return Instance.ExecuteHasAvailable(id);
        }

        /// <summary>
        /// Returns true if there's a pool of the given id
        /// </summary>
        public static bool HasPool(Enum enumId)
        {
            return HasPool(enumId.ToString());
        }

        /// <summary>
        /// Returns true if there's a pool of the given id
        /// </summary>
        public static bool HasPool(string id)
        {
            return Instance._pools.ContainsKey(id);
        }

        /// <summary>
        /// Go through each pool releasing all acquired objects
        /// </summary>
        public static void ReleaseAll()
        {
            foreach (var pool in Instance._pools.Values)
            {
                pool.ReleaseAll();
            }
        }
        /// <summary>
        /// Returns the total of objects in the given pool that are "in use"
        /// </summary>
        public static int TotalInUse(Enum enumId)
        {
            return TotalInUse(enumId.ToString());
        }


        /// <summary>
        /// Returns the total of objects in the given pool that are "in use"
        /// </summary>
        public static int TotalInUse(string id)
        {
            if (!Instance._pools.ContainsKey(id)) return 0;

            var pool = Instance._pools[id];
            return pool.TotalInUse;
        }

        #region Private Methods

        public GameObject[] ExecutePeek(string id)
        {
            var entries = _pools[id].Entries;
            return entries;
        }

        private void ExecuteCreatePool(string id, GameObject prefab, int size = 10)
        {
            var pool = new PrefabPool(prefab, size);
            _pools.Add(id, pool);
        }

        private GameObject ExecuteFetchAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return null;

            var pool = _pools[id];
            return pool.FetchAvailable();
        }

        private GameObject ExecuteFetchAvailable<T>(string id, out T poolable) where T : IPrefabPoolable
        {
            if (!_pools.ContainsKey(id))
            {
                poolable = default;
                return null;
            }

            var pool = _pools[id];
            return pool.FetchAvailable(out poolable);
        }

        private bool ExecuteHasAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return false;

            return _pools[id].HasAvailable();
        }
        #endregion
    }
}