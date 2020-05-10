using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class RangeSpawner : Spawner
    {
        public Vector3 range;

        void Start()
        {
            _timeStarted = Time.time;
        }

        void Update()
        {
            if (Time.time - _timeStarted >= frequency)
            {
                Spawn();
                _timeStarted = Time.time;
            }
        }

        protected override void Spawn()
        {
            if (SpawnSystem.Spawn(id, out Poolable gameObject))
            {
                var pos = gameObject.transform.position;
                pos.x += Random.Range(-range.x, range.x);
                pos.y += Random.Range(-range.y, range.y);
                pos.z += Random.Range(-range.z, range.z);
                gameObject.transform.position = pos;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, range);
        }
    }
}
