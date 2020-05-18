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
        private int _lastPointIndex;

        void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        public SpawnPoint FetchSpawnPoint(int index)
        {
            if (Total == -1) return null;

            return _spawnPoints[index > Total ? 0 : index];
        }

        public SpawnPoint FetchFarSpawnPoint(bool useUnique = true)
        {
            var targetIndex = 0;
            var distance = 0f;

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (useUnique && i == _lastPointIndex) continue;

                var sp = _spawnPoints[i];
                if (sp.DistanceToPlayer > distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = i;
                }
            }

            _lastPointIndex = targetIndex;
            return _spawnPoints[targetIndex];
        }

        public SpawnPoint FetchCloseSpawnPoint(bool useUnique = true)
        {
            var targetIndex = 0;
            var distance = 1000f;

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                var spawnPoint = _spawnPoints[i];
                if (spawnPoint.DistanceToPlayer < distance)
                {
                    distance = spawnPoint.DistanceToPlayer;
                    targetIndex = i;
                }
            }

            _lastPointIndex = targetIndex;
            return _spawnPoints[targetIndex];
        }

        public bool IsSpawnPointValid(int index)
        {
            return index <= _spawnPoints.Length && index >= 0;
        }

        public int Total
        {
            get { return _spawnPoints != null ? _spawnPoints.Length : -1; }
        }

        public SpawnPoint[] SpawnPoints  => _spawnPoints;

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