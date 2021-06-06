using BrightLib.Pooling.Runtime;
using UnityEngine;

namespace BrightLib.Pooling.Samples.CubeSample
{
    public class CubePoolable : Poolable
    {
        public float lifeDuration = 1f;

        private float _timeBorn;

        private void Awake()
        {
            //Debug.Log($"{name} awake");
        }

        private void Start()
        {
            //Debug.Log($"{name} start");
        }

        private void OnEnable()
        {
            //Debug.Log($"{name} OnEnable");
        }

        private void OnDisable()
        {
            //Debug.Log($"{name} OnDisable");
        }

        protected override void OnAcquire()
        {
            Debug.Log($"{name} OnAcquire");
            _timeBorn = Time.time;
        }

        protected override void OnRelease()
        {
            Debug.Log($"{name} OnRelease");
        }

        private void Update()
        {
            if(Time.time - _timeBorn >= lifeDuration)
            {
                Release();
            }
        }

        

    }
}