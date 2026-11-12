using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.Shop.Contract;
using LeaseExtension.Wallet.Contract;
using MessagePipe;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopViewModel : IShopViewModel, IDisposable
    {
        private readonly IShopView _view;
        private readonly IPublisher<FocusedScene> _sceneRequestPublisher;
        private readonly IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> _models;
        private readonly IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel> _appliedCosmetics;
        private readonly IReadOnlyDictionary<CosmeticsTarget, IAppliedProductModel> _appliedProducts;
        private readonly IWalletModel _wallet;
        private readonly IShop _shop;
        private readonly IShopPreviewSpawner _preview;
        private readonly Dictionary<IProductModel, string> _productIds = new();
        private readonly List<IDisposable> _subscriptions = new();
        private readonly CancellationTokenSource _viewInitialization = new();
        private Dictionary<string, AsyncOperationHandle<IProductModel>> _products = new();
        private CosmeticsTarget _target;
        private IProductModel _selected;
        private int _requestVersion;
        private bool _isDisposed;

        public ShopViewModel(
            IShopView view,
            IPublisher<FocusedScene> sceneRequestPublisher,
            IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> models,
            IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel> appliedCosmetics,
            IReadOnlyDictionary<CosmeticsTarget, IAppliedProductModel> appliedProducts,
            IWalletModel wallet,
            IShop shop,
            IShopPreviewSpawner preview)
        {
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            _models = models;
            _appliedCosmetics = appliedCosmetics;
            _appliedProducts = appliedProducts;
            _wallet = wallet;
            _shop = shop;
            _preview = preview;
            InitializeView().Forget();
        }

        public void Dispose()
        {
            _isDisposed = true;
            _requestVersion++;
            _viewInitialization.Cancel();
            _viewInitialization.Dispose();
            foreach (IDisposable subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            if (_view.Root != null)
            {
                _view.Close.clicked -= OnCloseClicked;
                _view.Apply.clicked -= OnApplyClicked;
                _view.OnProductSelected -= OnProductSelected;
                _view.OnProductSelectionCleared -= ClearSelection;
                _view.CosmeticsTarget.UnregisterValueChangedCallback(OnCosmeticsTargetChanged);
            }

            ReleaseProducts();
        }

        private async UniTask InitializeView()
        {
            await UniTask.WaitWhile(() => _view.Root == null, cancellationToken: _viewInitialization.Token);
            if (_view.CosmeticsTarget.value is not CosmeticsTarget)
            {
                _view.CosmeticsTarget.Init(default(CosmeticsTarget));
            }

            _view.Close.clicked += OnCloseClicked;
            _view.Apply.clicked += OnApplyClicked;
            _view.OnProductSelected += OnProductSelected;
            _view.OnProductSelectionCleared += ClearSelection;
            _view.CosmeticsTarget.RegisterValueChangedCallback(OnCosmeticsTargetChanged);
            _subscriptions.Add(_wallet.BalanceChanged.Subscribe(_ => UpdateButton()));
            foreach (KeyValuePair<CosmeticsTarget, ITargetProductListModel> entry in _models)
            {
                CosmeticsTarget target = entry.Key;
                _subscriptions.Add(entry.Value.ProductOwned.Subscribe(_ => OnProductOwned(target)));
                _subscriptions.Add(_appliedProducts[target].IdChanged.Subscribe(_ => OnAppliedProductChanged(target)));
            }

            UpdateProductList((CosmeticsTarget)_view.CosmeticsTarget.value);
        }

        private void OnCloseClicked()
        {
            _sceneRequestPublisher.Publish(FocusedScene.Game);
        }

        private void OnCosmeticsTargetChanged(ChangeEvent<Enum> evt)
        {
            UpdateProductList((CosmeticsTarget)evt.newValue);
        }

        private void UpdateProductList(CosmeticsTarget target)
        {
            _target = target;
            _requestVersion++;
            ClearSelection();
            _view.ClearProducts();
            ReleaseProducts();
            LoadProducts(target, _requestVersion).Forget();
        }

        private async UniTask LoadProducts(CosmeticsTarget target, int requestVersion)
        {
            Dictionary<string, AsyncOperationHandle<IProductModel>> products = new();
            bool isRetained = false;
            try
            {
                IEnumerable<string> ids = _models[target].Products.Select(product => product.AssetGUID)
                    .Concat(_models[target].OwnedProductIds).Append(_appliedProducts[target].Id).Distinct();
                foreach (string id in ids)
                {
                    products.Add(id, Addressables.LoadAssetAsync<IProductModel>(id));
                }

                await UniTask.WhenAll(products.Values.Select(handle => handle.ToUniTask()));
                if (!_isDisposed && requestVersion == _requestVersion)
                {
                    _products = products;
                    isRetained = true;
                    foreach (AssetReference reference in _models[target].Products)
                    {
                        _productIds.Add(products[reference.AssetGUID].Result, reference.AssetGUID);
                    }

                    foreach (string id in _models[target].OwnedProductIds)
                    {
                        IProductModel product = products[id].Result;
                        if (_productIds.ContainsKey(product))
                        {
                            _productIds[product] = id;
                        }
                    }

                    _view.SetProducts(_productIds.Keys);
                    RefreshOwnership();
                    IProductModel applied = products[_appliedProducts[target].Id].Result;
                    if (_productIds.ContainsKey(applied))
                    {
                        _view.SelectProduct(applied);
                    }
                }
            }
            finally
            {
                if (!isRetained)
                {
                    foreach (AsyncOperationHandle<IProductModel> handle in products.Values)
                    {
                        Addressables.Release(handle);
                    }
                }
            }
        }

        private void OnProductSelected(IProductModel product)
        {
            _selected = product;
            _appliedCosmetics[_target].Id = _productIds[product];
            _preview.Show(_target);
            UpdateButton();
        }

        private void ClearSelection()
        {
            _selected = null;
            _preview.Clear();
            UpdateButton();
        }

        private void OnApplyClicked()
        {
            IProductModel product = _selected;
            string productId = _productIds[product];
            if (_shop.TryAcquire(_target, product, productId))
            {
                _appliedProducts[_target].Id = productId;
            }
            UpdateButton();
        }

        private void RefreshOwnership()
        {
            IProductModel[] owned = _models[_target].OwnedProductIds.Select(id => _products[id].Result).ToArray();
            _view.RefreshProducts(owned);
            UpdateButton();
        }

        private void OnProductOwned(CosmeticsTarget target)
        {
            if (target == _target && _products.Count > 0)
            {
                RefreshOwnership();
            }
        }

        private void OnAppliedProductChanged(CosmeticsTarget target)
        {
            if (target == _target && _products.Count > 0)
            {
                IProductModel applied = _products[_appliedProducts[target].Id].Result;
                if (_productIds.ContainsKey(applied))
                {
                    _view.SelectProduct(applied);
                }
            }
        }

        private void UpdateButton()
        {
            if (_selected == null)
            {
                _view.Apply.text = "Apply";
                _view.Apply.SetEnabled(false);
            }
            else
            {
                bool owned = _models[_target].OwnedProductIds.Contains(_productIds[_selected]);
                _view.Apply.text = owned ? "Apply" : "Exchange";
                _view.Apply.SetEnabled(owned
                    ? _products[_appliedProducts[_target].Id].Result != _selected
                    : _wallet.Balance >= _selected.Price);
            }
        }

        private void ReleaseProducts()
        {
            foreach (AsyncOperationHandle<IProductModel> handle in _products.Values)
            {
                Addressables.Release(handle);
            }

            _products.Clear();
            _productIds.Clear();
        }
    }
}
