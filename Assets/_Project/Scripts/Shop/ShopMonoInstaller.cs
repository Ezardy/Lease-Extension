using System;
using System.Collections.Generic;
using LeaseExtension.Common.Service;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using LeaseExtension.Shop.Model;
using LeaseExtension.Shop.Save;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using Zenject;

namespace LeaseExtension.Shop
{
    internal class ShopMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private SpriteRenderer _wall;
        [Space, SerializeField]
        private SpriteRenderer _ruler;
        [SerializeField]
        private SpriteShapeController _rulerShadow;
        [SerializeField, Range(0, 1)]
        private float _rulerWidth = 1;
        [SerializeField, Min(0)]
        private float _rulerMinRatio;
        [SerializeField, Min(0)]
        private float _rulerMaxRatio = 1.8f;
        [SerializeField]
        private List<TargetProducts> _productsByTarget = new();

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_wall);
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_ruler, _rulerWidth, _rulerMinRatio, _rulerMaxRatio);
            Container.BindInterfacesTo<SpriteShapeWidthResizer>().AsSingle()
                .WithArguments(_rulerShadow, _rulerWidth + 0.02f, _rulerMinRatio, _rulerMaxRatio);
            InstallProductLists();
        }

        private void InstallProductLists()
        {
            Dictionary<CosmeticsTarget, ITargetProductListModel> models = new();
            foreach (TargetProducts targetProducts in _productsByTarget)
            {
                OwnedProductCollectionSave save = targetProducts.Save;
                if (!Container.IsValidating)
                {
                    save.Load();
                }

                TargetProductListModel model = new(targetProducts.Products, save.Data.Ids);
                models.Add(targetProducts.Target, model);
                save.Init(model);
                Container.BindInterfacesTo<OwnedProductCollectionSave>().FromInstance(save);
            }

            Container.BindInstance<IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel>>(models);
        }

        [Serializable]
        private class TargetProducts
        {
            public CosmeticsTarget Target;
            public List<AssetReferenceT<ProductModel>> Products;
            public OwnedProductCollectionSave Save;
        }
    }
}
