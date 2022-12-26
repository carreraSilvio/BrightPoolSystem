using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public sealed class Poolable : MonoBehaviour
    {
        /// <summary>
        /// Fired when the object is acquired from the pool
        /// </summary>
        public event Action<GameObject> OnAcquire;

        /// <summary>
        /// Fired when the object retuns to the pool
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
        /// <remarks>
        /// Call when you're done using it ie. a bullet finishes it's trajectory
        /// </remarks>
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