using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public sealed class Poolable : MonoBehaviour
    {
        /// <summary>
        /// Fired by the object once it has been aquired successfuly
        /// </summary>
        public event Action<GameObject> OnAcquire;

        /// <summary>
        /// Fired by the object once it has finished it's use and it becomes available for the pool
        /// </summary>
        public event Action<GameObject> OnRelease;

        public bool Acquired { get; private set; }

        /// <summary>
        /// Acquires the object from the pool.
        /// </summary>
        internal void Acquire()
        {
            if (Acquired)
            {
                return;
            }

            Acquired = true;
            gameObject.SetActive(true);
            OnAcquire?.Invoke(gameObject);
        }

        /// <summary>
        /// Returns the object to the pool and disables it.
        /// </summary>
        public void Release()
        {
            if (!Acquired)
            {
                return;
            }

            Acquired = false;
            gameObject.SetActive(false);
            OnRelease?.Invoke(gameObject);
        }

    }
}