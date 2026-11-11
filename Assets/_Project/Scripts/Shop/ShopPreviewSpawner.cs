using System;
using System.Collections.Generic;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeaseExtension.Shop
{
    internal class ShopPreviewSpawner : IShopPreviewSpawner, IDisposable
    {
        private readonly Transform _spawnPoint;
        private readonly IReadOnlyDictionary<CosmeticsTarget, GameObject> _prefabs;
        private GameObject _instance;

        public ShopPreviewSpawner(Transform spawnPoint, IReadOnlyDictionary<CosmeticsTarget, GameObject> prefabs)
        {
            _spawnPoint = spawnPoint;
            _prefabs = prefabs;
        }

        public void Show(CosmeticsTarget target, IProductModel product)
        {
            Clear();
            if (product != null && _spawnPoint && _prefabs.TryGetValue(target, out GameObject prefab) && prefab)
            {
                GameObject staging = new("Shop Preview Staging");
                staging.SetActive(false);
                _instance = Object.Instantiate(prefab, staging.transform, false);
                MonoBehaviour[] behaviours = _instance.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    behaviour.enabled = false;
                }

                foreach (ICosmeticsPreview preview in _instance.GetComponentsInChildren<ICosmeticsPreview>(true))
                {
                    if (preview.Target == target)
                    {
                        preview.SetPreview(product);
                    }
                }

                for (int i = behaviours.Length - 1; i >= 0; i--)
                {
                    Object.DestroyImmediate(behaviours[i]);
                }

                _instance.transform.SetParent(_spawnPoint, false);
                _instance.transform.localPosition = Vector3.zero;
                _instance.transform.localRotation = Quaternion.identity;
                _instance.transform.localScale = Vector3.one;
                _instance.SetActive(true);
                Object.Destroy(staging);
            }
        }

        public void Clear()
        {
            if (_instance)
            {
                _instance.SetActive(false);
                Object.Destroy(_instance);
                _instance = null;
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
