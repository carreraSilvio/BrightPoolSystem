using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PointSpawner : Spawner
    {
        [SerializeField]
        private SpawnPoint[] _spawnPoints = default;

        [SerializeField]
        private SpawnDistanceType _spawnDistance = default;

        [SerializeField, Tooltip("Will skip choosing the same spawn point in a row")]
        private bool _avoidRepeating = true;

        private int _lastSpawnPointIndex;

        void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
            if(_spawnPoints.Length == 0)
            {
                var go = new GameObject
                {
                    name = "SpawnPoint_" + _spawnPoints.Length
                };
                go.transform.SetParent(transform);
                go.AddComponent<SpawnPoint>();
                _spawnPoints = GetComponentsInChildren<SpawnPoint>();
            }
            
        }

        void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        protected override void Spawn()
        {
            if (SpawnSystem.Spawn(id, out Poolable gameObject))
            {
                var pos = FetchSpawnPointPosition(_spawnPoints, _spawnDistance);
                gameObject.transform.position = pos;
            }
        }

        public Vector3 FetchSpawnPointPosition(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (_avoidRepeating && i == _lastSpawnPointIndex) continue;

                var sp = spawnPoints[i];
                if (spawnDistance == SpawnDistanceType.Far)
                {
                    if (sp.DistanceToPlayer >= distance)
                    {
                        distance = sp.DistanceToPlayer;
                        targetIndex = i;
                    }
                }
                else if (spawnDistance == SpawnDistanceType.Close)
                {
                    if (sp.DistanceToPlayer <= distance)
                    {
                        distance = sp.DistanceToPlayer;
                        targetIndex = i;
                    }
                }
            }

            _lastSpawnPointIndex = targetIndex;
            var targetSpawnPoint = spawnPoints[targetIndex];
            targetSpawnPoint.MarkUse();
            return targetSpawnPoint.transform.position;
        }

        
    }

}
