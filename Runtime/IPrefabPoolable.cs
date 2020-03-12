using System;
using UnityEngine;

public interface IPrefabPoolable
{
    event Action<GameObject> onRelease;

    void Aquire();
    void Release();
}
