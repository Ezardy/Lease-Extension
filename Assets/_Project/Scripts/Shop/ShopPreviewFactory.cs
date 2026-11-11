using System.Collections.Generic;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Shop
{
    internal class ShopPreviewFactory : IFactory<CosmeticsTarget, GameObject>
    {
        private readonly DiContainer _container;
        private readonly Transform _spawnPoint;
        private readonly IReadOnlyDictionary<CosmeticsTarget, GameObject> _prefabs;

        public ShopPreviewFactory(
            DiContainer container,
            Transform spawnPoint,
            IReadOnlyDictionary<CosmeticsTarget, GameObject> prefabs)
        {
            _container = container;
            _spawnPoint = spawnPoint;
            _prefabs = prefabs;
        }

        public GameObject Create(CosmeticsTarget target)
        {
            GameObject instance = _container.InstantiatePrefab(_prefabs[target], _spawnPoint);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            return instance;
        }
    }
}
