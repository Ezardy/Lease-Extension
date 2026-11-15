using System;
using System.Collections.Generic;
using System.Linq;
using LeaseExtension.Common.Factory;
using LeaseExtension.Common.Service;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using LeaseExtension.Shop.Model;
using LeaseExtension.Shop.Save;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using Zenject;
using Object = UnityEngine.Object;

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
        [SerializeField] private Transform _previewSpawnPoint;

        public override void InstallBindings()
        {
            InstallBackground();
            InstallProductLists();
            InstallPreview();
            Container.BindInterfacesTo<Shop>().AsSingle();
        }

        private void InstallPreview()
        {
            Container.BindFactory<Object, GameObject, PrefabFactory.Factory>().FromFactory<PrefabFactory>();
            Dictionary<CosmeticsTarget, GameObject> prefabs = new();
            foreach (TargetProducts products in _productsByTarget)
            {
                prefabs.Add(products.Target, products.PreviewPrefab);
                Container.Bind<IAppliedCosmeticsModel>().WithId(products.Target)
                    .FromMethod(CreatePreviewCosmeticsModel).AsCached();
            }

            Container.Bind<IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel>>()
                .FromMethod(CreatePreviewCosmeticsModels).AsSingle();
            Container.Bind<IReadOnlyDictionary<CosmeticsTarget, IAppliedProductModel>>()
                .FromMethod(CreateAppliedProductModels).AsSingle();
            Container.BindInterfacesTo<ShopPreviewSpawner>().AsSingle().WithArguments(_previewSpawnPoint, prefabs);
        }

        private void InstallBackground()
        {
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_wall);
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_ruler, _rulerWidth, _rulerMinRatio, _rulerMaxRatio);
            Container.BindInterfacesTo<SpriteShapeWidthResizer>().AsSingle()
                .WithArguments(_rulerShadow, _rulerWidth + 0.02f, _rulerMinRatio, _rulerMaxRatio);
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

        private IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel> CreatePreviewCosmeticsModels(
            InjectContext context)
        {
            Dictionary<CosmeticsTarget, IAppliedCosmeticsModel> applied = new();
            foreach (TargetProducts products in _productsByTarget)
            {
                applied.Add(products.Target, context.Container.ResolveId<IAppliedCosmeticsModel>(products.Target));
            }

            return applied;
        }

        private IReadOnlyDictionary<CosmeticsTarget, IAppliedProductModel> CreateAppliedProductModels(
            InjectContext context)
        {
            Dictionary<CosmeticsTarget, IAppliedProductModel> applied = new();
            foreach (TargetProducts products in _productsByTarget)
            {
                IAppliedCosmeticsModel cosmetics = context.Container.ParentContainers.Single()
                    .ResolveId<IAppliedCosmeticsModel>(products.Target);
                applied.Add(products.Target, new AppliedProductModel(cosmetics));
            }

            return applied;
        }

        private IAppliedCosmeticsModel CreatePreviewCosmeticsModel(InjectContext context)
        {
            IAppliedCosmeticsModel applied = context.Container.ParentContainers.Single()
                .ResolveId<IAppliedCosmeticsModel>(context.Identifier);
            return new PreviewCosmeticsModel(applied.Id);
        }

        [Serializable]
        private class TargetProducts
        {
            public CosmeticsTarget Target;
            public List<AssetReferenceT<ProductModel>> Products;
            public OwnedProductCollectionSave Save;
            public GameObject PreviewPrefab;
        }
    }
}