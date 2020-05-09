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
         => PoolSystem.TotalInUse(id);

        #region Spawn (out Poolable)

        public static bool Spawn(Enum idEnum, SpawnPointController spController, PointSpawnMode pointSpawnerMode, out Poolable poolable)
            => Spawn(idEnum.ToString(), spController, pointSpawnerMode, out poolable);
        public static bool Spawn(Enum idEnum, Transform transform, out Poolable poolable)
            => Instance.ExecuteSpawn(idEnum.ToString(), transform.position, out poolable);
        public static bool Spawn(Enum idEnum, out Poolable poolable)
            => Instance.ExecuteSpawn(idEnum.ToString(), Vector3.zero, out poolable);

        public static bool Spawn(string id, SpawnPointController spController, PointSpawnMode pointSpawnerMode, out Poolable poolable)
        {
            var position = pointSpawnerMode == PointSpawnMode.Close ?
                spController.FetchCloseSpawnPoint().transform.position :
                spController.FetchFarSpawnPoint().transform.position;
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
            poolable.gameObject.SetActive(true);
            poolable.gameObject.transform.position = position;

            return true;
        }

        #endregion

        #region Spawn (NO out poolable)

        public bool Spawn(Enum spawnerIdEnum, SpawnPointController spController, PointSpawnMode pointSpawnerMode)
            => Spawn(spawnerIdEnum.ToString(), spController, pointSpawnerMode);
        public bool Spawn(Enum spawnerIdEnum, Transform transform)
            => Spawn(spawnerIdEnum.ToString(), transform.position);
        public bool Spawn(Enum spawnerIdEnum)
            => Spawn(spawnerIdEnum.ToString(), Vector3.zero);

        public bool Spawn(string spawnerId, SpawnPointController spController, PointSpawnMode pointSpawnerMode)
        {
            var position = pointSpawnerMode == PointSpawnMode.Close ?
                spController.FetchCloseSpawnPoint().transform.position :
                spController.FetchFarSpawnPoint().transform.position;
            return Spawn(spawnerId, position);
        }

        public bool Spawn(string spawnerId, Transform transform)
            => Spawn(spawnerId, transform.position);
        public bool Spawn(string spawnerId)
            => Spawn(spawnerId, Vector3.zero);

        public bool Spawn(string spawnerId, Vector3 position)
        {
            if (!PoolSystem.FetchAvailable(spawnerId, out GameObject gameObject))
            {
                return false;
            }

            gameObject.transform.position = position;
            return true;
        }

        #endregion


    }
}