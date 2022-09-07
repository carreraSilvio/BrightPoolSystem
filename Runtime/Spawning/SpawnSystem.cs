using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Responsible for knowing how to spawn poolables out of the <see cref="PoolSystem"/>
    /// </summary>
    public sealed class SpawnSystem
    {
        private SpawnSystem() { }

        public static int TotalSpawned(Enum id)
        {
            return PoolSystem.TotalAcquired(id);
        }

        public static int TotalSpawned(string id)
        {
            return PoolSystem.TotalAcquired(id);
        }

        #region Spawn (out Poolable)

        public static bool Spawn(Enum idEnum, SpawnPointController spawnPointController, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            return Spawn(idEnum.ToString(), spawnPointController, spawnDistance, out poolable);
        }

        public static bool Spawn(Enum idEnum, SpawnPointController spawnPointController, out Poolable poolable)
        {
            return Spawn(idEnum.ToString(), spawnPointController, SpawnDistanceType.Far, out poolable);
        }

        public static bool Spawn(Enum idEnum, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            return Spawn(idEnum.ToString(), spawnPoints, spawnDistance, out poolable);
        }

        public static bool Spawn(Enum idEnum, Transform transform, out Poolable poolable)
        {
            return ExecuteSpawn(idEnum.ToString(), transform.position, out poolable);
        }

        public static bool Spawn(Enum idEnum, out Poolable poolable)
        {
            return ExecuteSpawn(idEnum.ToString(), Vector3.zero, out poolable);
        }

        public static bool Spawn(string id, SpawnPointController spawnPointController, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            var spawnPoint = spawnPointController.GetSpawnPoint(spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.transform.position, out poolable);
        }

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            var spawnPoint = GetSpawnPoint(spawnPoints, spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.transform.position, out poolable);
        }

        public static bool Spawn(string enumId, Transform transform, out Poolable poolable)
        {
            return ExecuteSpawn(enumId, transform.position, out poolable);
        }

        public static bool Spawn(string id, out Poolable poolable)
        {
            return ExecuteSpawn(id, Vector3.zero, out poolable);
        }

        private static bool ExecuteSpawn(string id, Vector3 position, out Poolable poolable)
        {
            if (!PoolSystem.FetchAvailable(id, out GameObject gameObject))
            {
                Debug.LogWarning($"Poolable [{id}] was not found.");
                poolable = default;
                return false;
            }

            poolable = gameObject.GetComponentInChildren<Poolable>();
            gameObject.transform.position = position;

            return true;
        }

        #endregion

        #region Spawn

        public static bool Spawn(Enum idEnum, SpawnPointController spawnPointController)
        {
            return Spawn(idEnum.ToString(), spawnPointController.SpawnPoints, SpawnDistanceType.Far);
        }

        public static bool Spawn(Enum idEnum, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            return Spawn(idEnum.ToString(), spawnPoints, spawnDistance);
        }

        public static bool Spawn(Enum idEnum, Transform transform)
        {
            return ExecuteSpawn(idEnum.ToString(), transform.position);
        }

        public static bool Spawn(Enum idEnum)
        {
            return ExecuteSpawn(idEnum.ToString(), Vector3.zero);
        }

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            var spawnPoint = GetSpawnPoint(spawnPoints, spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.transform.position);
        }

        public static bool Spawn(string id, Transform transform)
        {
            return ExecuteSpawn(id, transform.position);
        }

        public static bool Spawn(string id)
        {
            return ExecuteSpawn(id, Vector3.zero);
        }

        public static bool ExecuteSpawn(string id, Vector3 position)
        {
            if (!PoolSystem.FetchAvailable(id, out GameObject gameObject))
            {
                return false;
            }

            gameObject.transform.position = position;
            return true;
        }

        #endregion

        public static SpawnPoint GetSpawnPoint(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            int targetIndex;
            if (spawnDistance == SpawnDistanceType.Far)
            {
                targetIndex = GetFarthestSpawnPoint(spawnPoints, new List<int>());
            }
            else
            {
                targetIndex = GetClosestSpawnPoint(spawnPoints, new List<int>());
            }

            var targerSpawnPoint = spawnPoints[targetIndex];
            targerSpawnPoint.MarkUse();
            return targerSpawnPoint;
        }

        /// <summary>
        /// Get the farthest spawnPoint from the player
        /// </summary>
        public static int GetFarthestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex))
                {
                    continue;
                }

                var spawnPoint = spawnPoints[spawnPointIndex];
                if (spawnPoint.DistanceToPlayer >= distance)
                {
                    distance = spawnPoint.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Get the closest spawnPoint fromn the player
        /// </summary>
        public static int GetClosestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = 9999f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex))
                {
                    continue;
                }

                var spawnPoint = spawnPoints[spawnPointIndex];
                if (spawnPoint.DistanceToPlayer <= distance)
                {
                    distance = spawnPoint.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Get a random spawn point
        /// </summary>
        public static int GetRandomSpawnPoint(SpawnPoint[] spawnPoints)
        {
            var targetIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);

            return targetIndex;
        }

    }
}