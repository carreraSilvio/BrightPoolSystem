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
        private Dictionary<string, Pool> _pools;

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
        }

        /// <summary>
        /// Create a new pool and add it to list
        /// </summary>
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
        public static bool TryFetchAvailable(string id, out GameObject gameObject)
        {
            var result = HasAvailable(id);
            gameObject = result ? FetchAvailable(id) : default;

            return result;
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static bool TryFetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            component = default;
            var result = HasAvailable(id);
            if(result) FetchAvailable(id, out component);

            return result;
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
        public static GameObject FetchAvailable<T>(Enum enumId, out T component) where T : MonoBehaviour
        {
            return FetchAvailable(enumId.ToString(), out component);
        }

        /// <summary>
        /// Searches for an available object and if it finds one, aquires it and returns.
        /// </summary>
        public static GameObject FetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            return Instance.ExecuteFetchAvailable(id, out component);
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
        public static int InUseTotal(Enum enumId)
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
            return pool.InUseTotal;
        }

        public GameObject[] ExecutePeek(string id)
        {
            var entries = _pools[id].Entries;
            return entries;
        }

        #region Private Methods

        private void ExecuteCreatePool(string id, GameObject prefab, int size = 10)
        {
            var pool = new Pool(prefab, size);
            _pools.Add(id, pool);
        }

        private GameObject ExecuteFetchAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return null;

            var pool = _pools[id];
            return pool.FetchAvailable();
        }

        private GameObject ExecuteFetchAvailable<T>(string id, out T component) where T : MonoBehaviour
        {
            if (!_pools.ContainsKey(id))
            {
                component = default;
                return null;
            }

            var pool = _pools[id];
            return pool.FetchAvailable(out component);
        }

        private bool ExecuteHasAvailable(string id)
        {
            if (!_pools.ContainsKey(id)) return false;

            return _pools[id].HasAvailable();
        }

        #endregion
    }
}