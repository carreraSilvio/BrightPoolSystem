using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// A point on the game that can be used to spawn game object
    /// </summary>
    public sealed class SpawnPoint : MonoBehaviour
    {
        public float DistanceToPlayer => Vector3.Distance(_player.transform.position, transform.position);
        public float LastTimeUsed { get; private set; }

        private static GameObject _player;

        private void Start()
        {
            ValidatePlayerReference();
        }

        private void OnEnable()
        {
            ValidatePlayerReference();
        }

        public void MarkUse()
        {
            LastTimeUsed = Time.time;
        }

        private void ValidatePlayerReference()
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
                if (_player == null)
                {
                    _player = gameObject;
                    Debug.LogError("Player not found. Using another transform to avoid null refs");
                }
            }
        }

        #region Debug
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
        #endregion
    }
}