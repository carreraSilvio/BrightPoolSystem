using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PoolTracker
    {
        public GameObject FindMainRoot(Pool pool)
        {
            if (pool.MainRoot != null) return pool.MainRoot;

            var mainRoot = GameObject.Find("PoolSystem");
            if (mainRoot == null)
            {
                mainRoot = new GameObject("PoolSystem");
                mainRoot.transform.SetAsLastSibling();
            }
            return mainRoot;
        }

        public GameObject FindLocalRoot(Pool pool, GameObject prefab)
        {
            if (pool.LocalRoot != null) return pool.LocalRoot;

            var localRoot = GameObject.Find(prefab.name + "Pool");
            if (localRoot == null)
            {
                localRoot = new GameObject(prefab.name + "Pool");
            }
            localRoot.transform.SetParent(pool.MainRoot.transform);

            return localRoot;
        }
    }
}