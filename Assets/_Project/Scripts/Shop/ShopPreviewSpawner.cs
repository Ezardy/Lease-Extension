using System;
using System.Collections.Generic;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace LeaseExtension.Shop
{
    internal class ShopPreviewSpawner : IShopPreviewSpawner, IDisposable
    {
        private readonly DiContainer _container;
        private readonly Transform _spawnPoint;
        private readonly IReadOnlyDictionary<CosmeticsTarget, GameObject> _prefabs;
        private GameObject _instance;
        private CosmeticsTarget? _target;

        public ShopPreviewSpawner(
            DiContainer container,
            Transform spawnPoint,
            IReadOnlyDictionary<CosmeticsTarget, GameObject> prefabs)
        {
            _container = container;
            _spawnPoint = spawnPoint;
            _prefabs = prefabs;
        }

        public void Show(CosmeticsTarget target, string productId)
        {
            _container.ResolveId<IAppliedCosmeticsModel>(target).Id = productId;
            if (_target != target)
            {
                Clear();
                _instance = _container.InstantiatePrefab(_prefabs[target], _spawnPoint);
                _instance.transform.localPosition = Vector3.zero;
                _instance.transform.localRotation = Quaternion.identity;
                _instance.transform.localScale = Vector3.one;
                _target = target;
            }
        }

        public void Clear()
        {
            if (_target.HasValue)
            {
                _instance.SetActive(false);
                Object.Destroy(_instance);
                _instance = null;
                _target = null;
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
