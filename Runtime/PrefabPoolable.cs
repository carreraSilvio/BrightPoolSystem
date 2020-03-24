using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PrefabPoolable : MonoBehaviour, IPoolable
    {
        public event Action<GameObject> onRelease;

        public virtual void Aquire()
        {
            gameObject.SetActive(true);
        }

        public virtual void Release()
        {
            gameObject.SetActive(false);
            onRelease?.Invoke(gameObject);
        }
    }
}