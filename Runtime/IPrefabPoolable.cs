using System;
using UnityEngine;

public interface IPrefabPoolable
{
    /// <summary>
    /// Fired by the object once it has finished it's use and it becames available for the pool
    /// </summary>
    event Action<GameObject> onRelease;

    /// <summary>
    /// Called when the object is available and someone requests it. Use as soft init.
    /// </summary>
    void Aquire();

    /// <summary>
    /// Called when object is returned to the pool. Use it to clean up and or hide the object
    /// </summary>
    void Release();
}