using UnityEngine;

/// <summary>
/// Knows the distance to the player and if it was used to spawn recently or not
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    private static GameObject _playerChar;
    private float _distanceToPlayer;
    private float _lastTimeUsed;

    void OnEnable()
    {
        _playerChar = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _distanceToPlayer = Vector3.Distance(_playerChar.transform.position, transform.position);
    }

    public void MarkUse()
    {
        _lastTimeUsed = Time.time;
    }

    public float DistanceToPlayer { get => _distanceToPlayer;  }
    public float LastTimeUsed { get => _lastTimeUsed; set => _lastTimeUsed = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}