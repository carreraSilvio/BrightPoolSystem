using BrightLib.Pooling.Runtime;
using System;
using UnityEngine;

namespace BrightLib.Pooling.Samples.CubeSample
{
    public class CubePoolable : Poolable
    {
        public float lifeDuration = 1f;

        private float _timeBorn;

        private void Awake()
        {
            //Debug.Log("cube awake");
        }

        private void Start()
        {
            //Debug.Log("cube start");
        }

        private void Update()
        {
            if(Time.time - _timeBorn >= lifeDuration)
            {
                Release();
            }
        }

        private void OnEnable()
        {
            _timeBorn = Time.time;
        }

    }
}