using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class SpawnSystem
    {
        private static SpawnSystem _instance;

        private static SpawnSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpawnSystem();
                }

                return _instance;
            }
        }

        public static int TotalSpawned(string id)
            => PoolSystem.TotalAquired(id);

        #region Spawn (out Poolable)

        public static bool Spawn(Enum idEnum, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
            => Spawn(idEnum.ToString(), spawnPoints, spawnDistance, out poolable);
        public static bool Spawn(Enum idEnum, Transform transform, out Poolable poolable)
            => Instance.ExecuteSpawn(idEnum.ToString(), transform.position, out poolable);
        public static bool Spawn(Enum idEnum, out Poolable poolable)
            => Instance.ExecuteSpawn(idEnum.ToString(), Vector3.zero, out poolable);

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            var position = SpawnerUtils.FetchSpawnPointPosition(spawnPoints, spawnDistance);
            return Instance.ExecuteSpawn(id, position, out poolable);
        }

        public static bool Spawn(string enumId, Transform transform, out Poolable poolable)
           => Instance.ExecuteSpawn(enumId, transform.position, out poolable);
        public static bool Spawn(string id, out Poolable poolable)
            => Instance.ExecuteSpawn(id, Vector3.zero, out poolable);

        private bool ExecuteSpawn(string id, Vector3 position, out Poolable poolable)
        {
            if(!PoolSystem.FetchAvailable(id, out GameObject gameObject))
            {
                poolable = default;
                return false;
            }

            poolable = gameObject.GetComponentInChildren<Poolable>();
            gameObject.transform.position = position;

            return true;
        }

        #endregion

        #region Spawn (NO out poolable)
        
        public bool Spawn(Enum idEnum, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
            => Spawn(idEnum.ToString(), spawnPoints, spawnDistance);
        public bool Spawn(Enum idEnum, Transform transform)
            => Spawn(idEnum.ToString(), transform.position);
        public bool Spawn(Enum idEnum)
            => Spawn(idEnum.ToString(), Vector3.zero);

        public bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            var position = SpawnerUtils.FetchSpawnPointPosition(spawnPoints, spawnDistance);
            return Spawn(id, position);
        }

        public bool Spawn(string id, Transform transform)
            => Spawn(id, transform.position);
        public bool Spawn(string id)
            => Spawn(id, Vector3.zero);

        public bool Spawn(string id, Vector3 position)
        {
            if (!PoolSystem.FetchAvailable(id, out GameObject gameObject))
            {
                return false;
            }

            gameObject.transform.position = position;
            return true;
        }

        #endregion


    }
}