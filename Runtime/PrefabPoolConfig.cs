using UnityEngine;

[System.Serializable]
public class PrefabPoolConfig
{
    public IPrefabPoolable prefabPoolable;
    public GameObject prefab;
    public int size = 10;
}