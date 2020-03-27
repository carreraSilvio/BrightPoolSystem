using BrightLib.Pooling.Runtime;
using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : PrefabPoolable
    {
        [SerializeField]
        private Rigidbody _rigidbody = default;

        private float _range;
        private Vector3 _positionFired;

        void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _rigidbody.velocity = Vector3.zero;
        }

        private void Update()
        {
            if (_rigidbody.velocity.sqrMagnitude > 0.01f)
            {
                if(Vector3.Distance(_positionFired, transform.position) >= _range)
                {
                    gameObject.SetActive(false);
                }
            }
        }


        public void Fire(Vector3 force, float range)
        {
            _positionFired = transform.position;
            _range = range;

            _rigidbody.AddForce(force);
        }
    }
}