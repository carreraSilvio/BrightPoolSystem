using System;
using System.Collections.Generic;

public class Pool<T> where T : IPoolable
{
    private List<T> _entries;
    private Queue<T> _available;

    public Pool(int size = 10)
    {
        _entries = new List<T>(size);
        _available = new Queue<T>(size);
        Create(size);
    }

    private void Create(int amount)
    {
        while (amount > 0)
        {
            var entry = (T)Activator.CreateInstance(typeof(T));
            entry.onRelease += HandleEntryRelease;
            _entries.Add(entry);
            _available.Enqueue(entry);

            amount--;
        }
    }

    private void HandleEntryRelease(IPoolable poolable)
    {
        _available.Enqueue((T)poolable);
    }

    public bool HasAvailable()
    {
        return (_available.Count > 0);
    }

    public T FetchAvailable()
    {
        var entry = _available.Dequeue();
        entry.Aquire();
        return entry;
    }

    public void ReleaseAll()
    {
        foreach(var entry in _entries)
        {
            entry.Release();
        }
    }
}