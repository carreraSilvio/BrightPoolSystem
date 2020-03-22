using BrightLib.Pooling.Runtime;
using System;
using UnityEngine;

namespace BrightLib.Pooling.Samples.CubeSample
{
    public class CubePoolable : MonoBehaviour, IPrefabPoolable
    {
        public event Action<GameObject> onRelease;

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

        public void Aquire()
        {
            _timeBorn = Time.time;
            gameObject.SetActive(true);
        }

        public void Release()
        {
            gameObject.SetActive(false);
            onRelease?.Invoke(gameObject);
        }
    }
}