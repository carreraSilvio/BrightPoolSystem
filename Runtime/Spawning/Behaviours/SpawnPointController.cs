using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Controls more than one SpawnPoint. Easy entry point to fetch lastUsed or closest/fartest to player
    /// </summary>
    public class SpawnPointController : MonoBehaviour
    {
        [SerializeField]
        private SpawnPoint[] _spawnPoints = default;

        private int _lastIndexUsed;

        void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        public SpawnPoint FetchSpawnPoint(SpawnDistanceType spawnDistanceType)
        {
            if (spawnDistanceType == SpawnDistanceType.Far)
            {
                return FetchFarthestSpawnPoint();
            }
            else if (spawnDistanceType == SpawnDistanceType.Random)
            {
                return FetchRandomSpawnPoint();
            }

            return FetchClosestSpawnPoint();
        }

        public SpawnPoint FetchFarthestSpawnPoint()
        {
            var spawnPointIndex = SpawnerUtils.FetchFarthestSpawnPoint(_spawnPoints, _lastIndexUsed);
            _lastIndexUsed = spawnPointIndex;
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint FetchClosestSpawnPoint()
        {
            var spawnPointIndex = SpawnerUtils.FetchClosestSpawnPoint(_spawnPoints, _lastIndexUsed);
            _lastIndexUsed = spawnPointIndex;
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint FetchRandomSpawnPoint()
        {
            var spawnPointIndex = SpawnerUtils.FetchRandomSpawnPoint(_spawnPoints);
            _lastIndexUsed = spawnPointIndex;
            return _spawnPoints[spawnPointIndex];
        }

        public int Total
        {
            get { return _spawnPoints != null ? _spawnPoints.Length : -1; }
        }

        public SpawnPoint[] SpawnPoints => _spawnPoints;

        void OnDrawGizmosSelected()
        {
            if (_spawnPoints == null) return;

            Gizmos.color = Color.yellow;
            foreach (var point in _spawnPoints)
            {
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }
    }
}