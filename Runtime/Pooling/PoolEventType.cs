namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Use it with <see cref="PoolSystem.AddListener(string, PoolEventType, PoolAction)"/>
    /// </summary>
    public enum PoolEventType
    {
        OnAcquire,
        OnRelease
    }
}