using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// A point on the game that can be used to spawn a game object
    /// </summary>
    public sealed class SpawnPoint : MonoBehaviour
    {
        public bool IsPlayerOutsideSafeSpawnDistance => DistanceToPlayer >= SafeSpawnDistance;
        public float DistanceToPlayer => Vector3.Distance(_player.transform.position, transform.position);
        public float LastTimeUsed { get; private set; }
        public int TimesUsed { get; private set; }
        public float SafeSpawnDistance => _safeSpawnDistance;

        [SerializeField]
        [Tooltip("If the player is within this distance, don't use this spawn point.")]
        private float _safeSpawnDistance = 3f;

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
            TimesUsed++;
        }

        public void ClearUse()
        {
            TimesUsed = 0;
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
            Gizmos.DrawWireSphere(transform.position, _safeSpawnDistance);
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position, $"{name}");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _safeSpawnDistance);
        }
#endif

    }
}