using UnityEngine;

/// <summary>
/// Knows the distance to the player and last time it was used to spawn
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    private static GameObject _playerChar;

    private float _distanceToPlayer;
    private float _lastTimeUsed;

    void OnEnable()
    {
        if (_playerChar == null) _playerChar = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _distanceToPlayer = Vector3.Distance(_playerChar.transform.position, transform.position);
    }

    public void MarkUse()
    {
        _lastTimeUsed = Time.time;
    }

    public float DistanceToPlayer { get => _distanceToPlayer; }
    public float LastTimeUsed { get => _lastTimeUsed; set => _lastTimeUsed = value; }

    public Vector3 Position { get => transform.position; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}