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

        public GameObject FindLocalRoot(GameObject prefab)
        {
            var localRoot = GameObject.Find(prefab.name + "Pool");
            if (localRoot == null)
            {
                localRoot = new GameObject(prefab.name + "Pool");
            }
            localRoot.transform.SetParent(_mainRoot.transform);

            return localRoot;
        }
    }
}