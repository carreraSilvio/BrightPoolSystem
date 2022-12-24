using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// A point on the game that can be used to spawn a game object
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

        private static void ValidatePlayerReference()
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
                if (_player == null)
                {
                    _player = new GameObject("Fake Player");
                    Debug.LogError("Player not found. Using another transform to avoid null refs");
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 1f);
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position, $"{name}");
        }
#endif

    }
}