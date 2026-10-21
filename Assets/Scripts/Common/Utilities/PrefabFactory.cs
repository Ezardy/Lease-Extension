using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Utilities
{
    [UsedImplicitly]
    public class PrefabFactory : IFactory<Object, GameObject>
    {
        private readonly DiContainer _container;

        public PrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject Create(Object prefab)
        {
            return _container.InstantiatePrefab(prefab);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<Object, GameObject>
        {}
    }
}