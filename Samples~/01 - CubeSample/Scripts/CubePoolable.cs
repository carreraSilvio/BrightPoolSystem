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

        public override void OnAquire()
        {
            Debug.Log($"{name} OnAquire");
            _timeBorn = Time.time;
        }

        public override void OnRelease()
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