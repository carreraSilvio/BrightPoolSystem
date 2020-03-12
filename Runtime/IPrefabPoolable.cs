using System;
using UnityEngine;

public interface IPrefabPoolable
{
    event Action<GameObject> onRelease;

    /// <summary>
    /// Called when the object is available and someone requests it. Use as soft init.
    /// </summary>
    void Aquire();

    /// <summary>
    /// Called when object is returned to the pool. Use it to clean up.
    /// </summary>
    void Release();
}
