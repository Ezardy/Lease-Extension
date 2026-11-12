using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Common.Factory;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeaseExtension.Shop
{
    [UsedImplicitly]
    internal class ShopPreviewSpawner : IShopPreviewSpawner, IDisposable
    {
        private readonly Transform _spawnPoint;
        private readonly IReadOnlyDictionary<CosmeticsTarget, GameObject> _prefabs;
        private readonly PrefabFactory.Factory _factory;
        private GameObject _instance;
        private CosmeticsTarget? _target;

        public ShopPreviewSpawner(Transform spawnPoint, IReadOnlyDictionary<CosmeticsTarget, GameObject> prefabs,
            PrefabFactory.Factory factory)
        {
            _spawnPoint = spawnPoint;
            _prefabs = prefabs;
            _factory = factory;
        }

        public void Show(CosmeticsTarget target)
        {
            if (_target != target)
            {
                Clear();
                _instance = _factory.Create(_prefabs[target]);
                _instance.transform.SetParent(_spawnPoint, false);
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
            if (_instance)
            {
                Clear();
            }
        }
    }
}