using UnityEngine.Events;

namespace BrightLib.Pooling.Runtime
{
    /// <summary>
    /// Pool event with arguments: (int) Pool Size, (int) Total In Use
    /// </summary>
    public class PoolEvent : UnityEvent<int, int> { };
}