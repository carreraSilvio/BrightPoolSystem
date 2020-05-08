using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PoolTracker
    {
        private static readonly string _kMainRootName = "PoolSystem";
        private GameObject _mainRoot;

        private GameObject CreateMainRoot()
        {
            var mainRoot = new GameObject(_kMainRootName);
            mainRoot.transform.SetAsLastSibling();
            return mainRoot;
        }

        public GameObject FindMainRoot()
        {
            if(_mainRoot == null)
            {
                _mainRoot = GameObject.Find(_kMainRootName);
                if (_mainRoot == null)
                {
                    _mainRoot = CreateMainRoot();
                }
            }
            return _mainRoot;
        }

        public GameObject FindLocalRoot(Pool pool)
        {
            if (pool.LocalRoot != null) return pool.LocalRoot;

            var localRoot = GameObject.Find(pool.Prefab.name + "Pool");
            if (localRoot == null)
            {
                localRoot = new GameObject(pool.Prefab.name + "Pool");
            }
            localRoot.transform.SetParent(_mainRoot.transform);

            return localRoot;
        }
    }
}