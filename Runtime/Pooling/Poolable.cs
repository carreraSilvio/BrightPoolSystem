using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class Poolable : MonoBehaviour
    {
        private bool _aquired;

        public bool Aquired { get => _aquired;}

        /// <summary>
        /// Fired by the object once it has finished it's use and it becomes available for the pool
        /// </summary>
        public event Action<GameObject> onRelease;

        /// <summary>
        /// Aquires the object from the pool.
        /// </summary>
        internal void Aquire()
        {
            _aquired = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Returns the object to the pool and disables it.
        /// </summary>
        public void Release()
        {
            if (!_aquired) return;

            _aquired = false;
            gameObject.SetActive(false);
            onRelease?.Invoke(gameObject);
        }

    }
}