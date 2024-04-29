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

        [Header("Gizmos")]
        [SerializeField]
        private string _indentifier = "S";

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
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, _safeSpawnDistance);
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position, $"{_indentifier}{transform.GetSiblingIndex()}");

            Gizmos.color = Color.white;
            ////Draw "S"
            //float width = 0.5f;
            //float height = 1f;
            //// Define the points for drawing the letter "S"
            //Vector3[] points = new Vector3[6];

            //// Start point
            //points[0] = new Vector3(width, 0f, 0);

            //// Top curve of "S"
            //points[1] = new Vector3(0f, 0f, 0);

            //// Midpoint
            //points[2] = new Vector3(0f, -height * 0.5f, 0);

            //// Bottom curve of "S"
            //points[3] = new Vector3(width, -height * 0.5f, 0);

            //// End point
            //points[4] = new Vector3(width, -height, 0);

            //// End point
            //points[5] = new Vector3(0f, -height, 0);

            //// Draw the letter "S" using Gizmos.DrawLine
            //Vector3 offset = new Vector2(1.1f, 1f);
            //for (int i = 0; i < points.Length - 1; i++)
            //{
            //    Gizmos.DrawLine(transform.position + points[i] + offset, transform.position + points[i + 1] + offset);
            //}

            //Draw sphere around
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _safeSpawnDistance);
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position, $"{_indentifier}{transform.GetSiblingIndex()}");
            //UnityEditor.Handles.Label(transform.position, $"{name}");
        }
#endif

    }
}