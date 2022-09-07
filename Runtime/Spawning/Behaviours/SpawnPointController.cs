using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Responsible for controlling multiple spawn points
    /// </summary>
    public sealed class SpawnPointController : MonoBehaviour
    {
        public SpawnPoint[] SpawnPoints => _spawnPoints;

        [SerializeField]
        private SpawnPoint[] _spawnPoints = default;

        private readonly List<int> _lastIndexUsedList = new List<int>();

        private void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        private void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        public void ClearIndexUsedList()
        {
            _lastIndexUsedList.Clear();
        }

        public SpawnPoint GetSpawnPoint(SpawnDistanceType spawnDistanceType)
        {
            if (spawnDistanceType == SpawnDistanceType.Far)
            {
                return GetFarthestSpawnPoint();
            }
            else if (spawnDistanceType == SpawnDistanceType.Random)
            {
                return GetRandomSpawnPoint();
            }

            return GetClosestSpawnPoint();
        }

        public SpawnPoint GetFarthestSpawnPoint()
        {
            if(_lastIndexUsedList.Count == _spawnPoints.Length)
            {
                _lastIndexUsedList.Clear();
            }

            var spawnPointIndex = SpawnSystem.GetFarthestSpawnPoint(_spawnPoints, _lastIndexUsedList);
            _lastIndexUsedList.Add(spawnPointIndex);
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint GetClosestSpawnPoint()
        {
            if (_lastIndexUsedList.Count == _spawnPoints.Length)
            {
                _lastIndexUsedList.Clear();
            }

            var spawnPointIndex = SpawnSystem.GetClosestSpawnPoint(_spawnPoints, _lastIndexUsedList);
            _lastIndexUsedList.Add(spawnPointIndex);
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint GetRandomSpawnPoint()
        {
            var spawnPointIndex = SpawnSystem.GetRandomSpawnPoint(_spawnPoints);
            return _spawnPoints[spawnPointIndex];
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            for (int pointIndex = 0; pointIndex < _spawnPoints.Length - 1; pointIndex++)
            {
                var startPoint = _spawnPoints[pointIndex];
                var endPoint = _spawnPoints[pointIndex + 1];
                var midPoint = (startPoint.transform.position + endPoint.transform.position) / 2;
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(midPoint, $"{midPoint.magnitude:F2}");
                Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
            }
        }
#endif

    }
}