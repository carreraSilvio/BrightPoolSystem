using System.Collections.Generic;
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

        private List<int> _lastIndexUsedList = new List<int>();

        void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        public void ClearIndexUsedList()
        {
            _lastIndexUsedList.Clear();
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
            if(_lastIndexUsedList.Count == _spawnPoints.Length)
            {
                _lastIndexUsedList.Clear();
            }

            var spawnPointIndex = SpawnSystem.FetchFarthestSpawnPoint(_spawnPoints, _lastIndexUsedList);
            _lastIndexUsedList.Add(spawnPointIndex);
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint FetchClosestSpawnPoint()
        {
            if (_lastIndexUsedList.Count == _spawnPoints.Length)
            {
                _lastIndexUsedList.Clear();
            }

            var spawnPointIndex = SpawnSystem.FetchClosestSpawnPoint(_spawnPoints, _lastIndexUsedList);
            _lastIndexUsedList.Add(spawnPointIndex);
            return _spawnPoints[spawnPointIndex];
        }

        public SpawnPoint FetchRandomSpawnPoint()
        {
            var spawnPointIndex = SpawnSystem.FetchRandomSpawnPoint(_spawnPoints);
            return _spawnPoints[spawnPointIndex];
        }

        public int Total
        {
            get { return _spawnPoints != null ? _spawnPoints.Length : -1; }
        }

        public SpawnPoint[] SpawnPoints => _spawnPoints;

     
    }
}