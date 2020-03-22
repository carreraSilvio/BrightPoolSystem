using BrightLib.Pooling.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Samples.CubeSample
{
    public class CubeSampleMain : MonoBehaviour
    {
        public float frequency = 2f;
        public Vector3 spawnRange;
        private float _timeStarted;

        // Start is called before the first frame update
        void Start()
        {
            _timeStarted = Time.time;
        }

        // Spawn a cube every frequency seconds
        void Update()
        {
            if(Time.time - _timeStarted >= frequency)
            {
                Spawn();
                _timeStarted = Time.time;
            }
        }

        private void Spawn()
        {
            var poolableGamObj = PoolSystem.FetchAvailable<CubePoolable>("Cube", out CubePoolable poolable);
            if(poolable != null)
            {
                var pos = poolableGamObj.transform.position;
                pos.x += Random.Range(-spawnRange.x, spawnRange.x);
                pos.y += Random.Range(-spawnRange.y, spawnRange.y);
                pos.z += Random.Range(-spawnRange.z, spawnRange.z);
                poolableGamObj.transform.position = pos;
                poolable.Aquire();
            }
        }
    }
}