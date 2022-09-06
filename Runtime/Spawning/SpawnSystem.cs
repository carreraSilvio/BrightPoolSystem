using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public sealed class SpawnSystem
    {
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
            var spawnPoint = spawnPointController.FetchSpawnPoint(spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.Position, out poolable);
        }

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            var spawnPoint = GetSpawnPoint(spawnPoints, spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.Position, out poolable);
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
            return ExecuteSpawn(id, spawnPoint.Position);
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
                targetIndex = FetchFarthestSpawnPoint(spawnPoints, new List<int>());
            }
            else
            {
                targetIndex = FetchClosestSpawnPoint(spawnPoints, new List<int>());
            }

            var targerSpawnPoint = spawnPoints[targetIndex];
            targerSpawnPoint.MarkUse();
            return targerSpawnPoint;
        }

        /// <summary>
        /// Returns the farthest spawnPoint fromn the player
        /// </summary>
        public static int FetchFarthestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex)) continue;

                var sp = spawnPoints[spawnPointIndex];
                if (sp.DistanceToPlayer >= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Returns the closest spawnPoint fromn the player
        /// </summary>
        public static int FetchClosestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = 9999f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex)) continue;

                var sp = spawnPoints[spawnPointIndex];
                if (sp.DistanceToPlayer <= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Returns a random spawn point
        /// </summary>
        public static int FetchRandomSpawnPoint(SpawnPoint[] spawnPoints)
        {
            var targetIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);

            return targetIndex;
        }

    }
}