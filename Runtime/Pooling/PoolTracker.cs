using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public class PoolTracker
    {
        private static readonly string _kMainRootName = "Pools";
        private GameObject _mainRoot;

        private GameObject CreateMainRoot()
        {
            var mainRoot = new GameObject(_kMainRootName);
            mainRoot.transform.SetAsLastSibling();
            return mainRoot;
        }

        public GameObject FindMainRoot()
        {
            if (_mainRoot == null)
            {
                _mainRoot = GameObject.Find(_kMainRootName);
                if (_mainRoot == null)
                {
                    _mainRoot = CreateMainRoot();
                }
            }
            return _mainRoot;
        }

        public GameObject FindLocalRoot(string id)
        {
            var localRoot = GameObject.Find(id + "Pool");
            if (localRoot == null)
            {
                localRoot = new GameObject(id + "Pool");
            }
            localRoot.transform.SetParent(_mainRoot.transform);

            return localRoot;
        }
    }
}