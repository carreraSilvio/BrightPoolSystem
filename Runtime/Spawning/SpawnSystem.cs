using System;
using UnityEngine;
using System.Linq;

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
            return Spawn(id, spawnPointController.SpawnPoints, spawnDistance, out poolable);
        }

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance, out Poolable poolable)
        {
            var spawnPoint = GetSpawnPointAndMarkUse(spawnPoints, spawnDistance);
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

        public static bool Spawn(string id, SpawnPointController spawnPointController, int spawnPointIndex, out Poolable poolable)
        {
            if(spawnPointIndex >= spawnPointController.SpawnPoints.Length)
            {
                poolable = default;
                return false;
            }
            var spawnPoint =  spawnPointController.SpawnPoints[spawnPointIndex];
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.transform.position, out poolable);
        }

        private static bool ExecuteSpawn(string id, Vector3 position, out Poolable poolable)
        {
            if (!PoolSystem.FetchAvailable(id, out GameObject gameObject))
            {
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

        public static bool Spawn(Enum idEnum, Vector3 position)
        {
            return ExecuteSpawn(idEnum.ToString(), position);
        }

        public static bool Spawn(Enum idEnum)
        {
            return ExecuteSpawn(idEnum.ToString(), Vector3.zero);
        }

        public static bool Spawn(string id, SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            var spawnPoint = GetSpawnPointAndMarkUse(spawnPoints, spawnDistance);
            spawnPoint.MarkUse();
            return ExecuteSpawn(id, spawnPoint.transform.position);
        }

        public static bool Spawn(string id, Vector3 position)
        {
            return ExecuteSpawn(id, position);
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

        public static bool Spawn(Enum idEnum, Vector3 position, out GameObject gameObject)
        {
            return ExecuteSpawn(idEnum.ToString(), position, out gameObject);
        }

        public static bool ExecuteSpawn(string id, Vector3 position, out GameObject gameObject)
        {
            if (!PoolSystem.FetchAvailable(id, out gameObject))
            {
                Debug.LogWarning($"Can't spawn poolable of type [{id}], none available.");
                return false;
            }

            gameObject.transform.position = position;
            return true;
        }

        #endregion

        public static SpawnPoint GetSpawnPointAndMarkUse(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            SpawnPoint spawnPoint = null;
            switch (spawnDistance)
            {
                case SpawnDistanceType.Far:
                case SpawnDistanceType.Manual:
                    //Get the furthest, least used spawn point
                    spawnPoint = spawnPoints.Where(spawnPoint => spawnPoint.IsPlayerOutsideSafeSpawnDistance).
                        OrderByDescending(spawnPoint => spawnPoint.DistanceToPlayer).
                            OrderBy(spawnPoint => spawnPoint.TimesUsed).FirstOrDefault();
                    break;

                case SpawnDistanceType.Close:
                    //Get the closest, least used spawn point
                    spawnPoint = spawnPoints.Where(spawnPoint => spawnPoint.IsPlayerOutsideSafeSpawnDistance).
                        OrderBy(spawnPoint => spawnPoint.DistanceToPlayer).
                            OrderBy(spawnPoint => spawnPoint.TimesUsed).FirstOrDefault();
                    break;

                case SpawnDistanceType.Random:
                    spawnPoint = GetRandomSpawnPoint(spawnPoints.Where(spawnPoint => spawnPoint.IsPlayerOutsideSafeSpawnDistance).ToArray());
                    break;
                default:
                    break;
            }

            spawnPoint.MarkUse();
            return spawnPoint;
        }

        /// <summary>
        /// Get a random spawn point
        /// </summary>
        public static SpawnPoint GetRandomSpawnPoint(SpawnPoint[] spawnPoints)
        {
            var targetIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);
            return spawnPoints[targetIndex];
        }

    }
}