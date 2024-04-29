using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Defines an area that can be used by the Spawn System to spawn game objects
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SpawnArea : MonoBehaviour
    {
        public float DistanceToPlayer => Vector3.Distance(_player.transform.position, transform.position);
        public float LastTimeUsed { get; private set; }

        [SerializeField]
        private float _width;

        [SerializeField]
        private float _height;

        [SerializeField]
        private float _spawnDistanceFromPlayer;

        [SerializeField]
        private float _spawnDistanceFromLastPoint;

        [SerializeField]
        private List<Vector3> _spawnPositions ;

        private Vector2 _topLeft;
        private Vector2 _topRight;
        private Vector2 _botLeft;
        private Vector2 _botRight;

        private static GameObject _player;

        private void Awake()
        {
            _topLeft = new Vector2(transform.position.x - _width / 2f, transform.position.y + _height / 2f);
            _topRight = new Vector2(transform.position.x + _width / 2f, transform.position.y + _height / 2f);

            _botLeft = new Vector2(transform.position.x - _width / 2f, transform.position.y - _height / 2f);
            _botRight = new Vector2(transform.position.x + _width / 2f, transform.position.y - _height / 2f);
        }

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

        public Vector3 GetRandomSpawnPosition()
        {
            return new Vector3(1f, 1f, 1f); //tbd
        }

        private Vector3 GenerateSpawnPosition()
        {
            var x = Random.Range(transform.position.x - _width / 2f, transform.position.x + _width / 2f);
            var y = Random.Range(transform.position.y - _height / 2f, transform.position.y + _height / 2f);

            return new Vector3(x, y, 1f);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(_width, _height, 1f));
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position, $"{name}");

            //Draw Spawn Positions
            for (int i = 0; i < _spawnPositions.Count; i++)
            {
                Vector3 spawnPosition = _spawnPositions[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(spawnPosition, new Vector3(1f, 1f, 1f));
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(spawnPosition, $"Spawn Position {i+1}");
            }
        }

        [ContextMenu(nameof(GenerateSpawnPositions))]
        public void GenerateSpawnPositions()
        {
            _spawnPositions = new List<Vector3>();
            _spawnPositions.Clear();

            for (int i = 0; i < 30; i++)
            {
                _spawnPositions.Add(GenerateSpawnPosition());
            }
        }
#endif

    }
}