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
            if(PoolSystem.FetchAvailable("Cube", out GameObject cubeGameObject))
            {
                var pos = cubeGameObject.transform.position;
                pos.x += Random.Range(-spawnRange.x, spawnRange.x);
                pos.y += Random.Range(-spawnRange.y, spawnRange.y);
                pos.z += Random.Range(-spawnRange.z, spawnRange.z);
                cubeGameObject.transform.position = pos;
            }
        }
    }
}