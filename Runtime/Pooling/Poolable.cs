using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class Poolable : MonoBehaviour
    {
        /// <summary>
        /// Fired by the object once it has been aquired successfuly
        /// </summary>
        public event Action<GameObject> onAcquire;

        /// <summary>
        /// Fired by the object once it has finished it's use and it becomes available for the pool
        /// </summary>
        public event Action<GameObject> onRelease;

        public bool Acquired { get => _acquired; }

        private bool _acquired;

        /// <summary>
        /// Acquires the object from the pool.
        /// </summary>
        internal void Acquire()
        {
            _acquired = true;
            gameObject.SetActive(true);
            onAcquire?.Invoke(gameObject);
            OnAcquire();
        }

        /// <summary>
        /// Returns the object to the pool and disables it.
        /// </summary>
        public void Release()
        {
            if (!_acquired) return;

            _acquired = false;
            gameObject.SetActive(false);
            onRelease?.Invoke(gameObject);
            OnRelease();
        }

        /// <summary>
        /// Called after the object is Acquired.
        /// </summary>
        protected virtual void OnAcquire()
        {

        }

        /// <summary>
        /// Called after the object is Released.
        /// </summary>
        protected virtual void OnRelease()
        {

        }

    }
}