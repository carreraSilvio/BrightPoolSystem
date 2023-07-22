using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Responsible for knowing about multiple spawn points and keeping tracks of the last used ones.
    /// </summary>
    public sealed class SpawnPointController : MonoBehaviour
    {
        public SpawnPoint[] SpawnPoints => _spawnPoints;

        [SerializeField]
        private SpawnPoint[] _spawnPoints = default;

        private void Reset()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        private void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        public void ClearSpawnPointUsage()
        {
            foreach(var spawnPoint in _spawnPoints)
            {
                spawnPoint.ClearUse();
            }
        }

        public SpawnPoint GetSpawnPoint(SpawnDistanceType spawnDistanceType)
        {
            return SpawnSystem.GetSpawnPointAndMarkUse(_spawnPoints, spawnDistanceType);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if(_spawnPoints == null 
                || _spawnPoints.Length == 0)
            {
                return;
            }
            Gizmos.color = Color.yellow;

            for (int pointIndex = 0; pointIndex < _spawnPoints.Length - 1; pointIndex++)
            {
                DrawLineBetweenSpawnPoints(_spawnPoints[pointIndex], _spawnPoints[pointIndex + 1]);
            }
            DrawLineBetweenSpawnPoints(_spawnPoints[_spawnPoints.Length - 1], _spawnPoints[0]);
        }

        private void DrawLineBetweenSpawnPoints(SpawnPoint spawnPointA, SpawnPoint spawnPointB)
        {
            var midPoint = (spawnPointA.transform.position + spawnPointB.transform.position) / 2;
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(midPoint, $"{midPoint.magnitude:F2}");
            Gizmos.DrawLine(spawnPointA.transform.position, spawnPointB.transform.position);
        }
#endif

    }
}