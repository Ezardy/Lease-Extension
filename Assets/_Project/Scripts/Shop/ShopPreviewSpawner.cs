using System;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace LeaseExtension.Shop
{
    internal class ShopPreviewSpawner : IShopPreviewSpawner, IDisposable
    {
        private readonly IFactory<CosmeticsTarget, GameObject> _factory;
        private GameObject _instance;
        private CosmeticsTarget? _target;

        public ShopPreviewSpawner(IFactory<CosmeticsTarget, GameObject> factory)
        {
            _factory = factory;
        }

        public void Show(CosmeticsTarget target)
        {
            if (_target != target)
            {
                Clear();
                _instance = _factory.Create(target);
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
