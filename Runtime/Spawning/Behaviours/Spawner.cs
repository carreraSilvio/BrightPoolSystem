using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public abstract class Spawner : MonoBehaviour
    {
        public string id = "poolableId";

        public float frequency = 2f;
        protected float _timeStarted;

        protected abstract void Spawn();
    }

}
