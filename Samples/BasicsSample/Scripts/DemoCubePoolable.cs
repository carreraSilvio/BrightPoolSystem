using System;
using UnityEngine;

public class DemoCubePoolable : MonoBehaviour, IPrefabPoolable
{
    public event Action<GameObject> onRelease;

    private void Awake()
    {
        Debug.Log("cube aweake");
    }

    private void Start()
    {
        Debug.Log("cube start");
    }

    public void Aquire()
    {
        //soft init
    }

    public void Release()
    {
        onRelease?.Invoke(gameObject);
    }
}
