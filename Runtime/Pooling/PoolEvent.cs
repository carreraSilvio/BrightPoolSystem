namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Use it with <see cref="PoolSystem.AddListener(string, PoolEvent, PoolAction)"/>
    /// </summary>
    public enum PoolEvent
    {
        OnAquire,
        OnRelease
    }
}