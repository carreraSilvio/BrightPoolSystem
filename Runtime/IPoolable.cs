using System;

public interface IPoolable
{
    event Action<IPoolable> onRelease;

    void Aquire();
    void Release();
}
