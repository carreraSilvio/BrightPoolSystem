using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PointSpawner : Spawner
    {
        [SerializeField]
        private SpawnPoint[] _spawnPoints = default;

        [SerializeField]
        private PointSpawnMode _spawnMode = default;

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
                var pos = _spawnPoints[0].transform.position;
                gameObject.transform.position = pos;
            }
        }
    }

}
