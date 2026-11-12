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
        private readonly CancellationTokenSource _viewInitialization = new();
        private IDisposable _subscriptions;
        private AsyncOperationHandle<IList<IProductModel>> _productsHandle;
        private CosmeticsTarget _target;
        private VisualElement _selected;
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
            _subscriptions.Dispose();

            if (_view.Root != null)
            {
                _view.Close.clicked -= OnCloseClicked;
                _view.Apply.clicked -= OnApplyClicked;
                _view.OnProductSelected -= OnProductSelected;
                _view.OnProductDeselected -= ClearSelection;
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
            _view.OnProductDeselected += ClearSelection;
            _view.CosmeticsTarget.RegisterValueChangedCallback(OnCosmeticsTargetChanged);
            _subscriptions = _wallet.BalanceChanged.Subscribe(_ => UpdateButton());

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
            if (_models[target].Products.Count > 0)
            {
                LoadProducts(target, _requestVersion).Forget();
            }
        }

        private async UniTask LoadProducts(CosmeticsTarget target, int requestVersion)
        {
            AsyncOperationHandle<IList<IProductModel>> handle =
                Addressables.LoadAssetsAsync<IProductModel>(_models[target].Products, null,
                    Addressables.MergeMode.Union);
            bool isRetained = false;
            try
            {
                IList<IProductModel> products = await handle.ToUniTask();
                if (!_isDisposed && requestVersion == _requestVersion)
                {
                    _productsHandle = handle;
                    isRetained = true;
                    _view.SetProducts(products, (v, m) => v.dataSource = m);
                    RefreshOwnership();
                    SelectAppliedProduct();
                }
            }
            finally
            {
                if (!isRetained)
                {
                    Addressables.Release(handle);
                }
            }
        }

        private void OnProductSelected(VisualElement card)
        {
            _selected = card;
            _appliedCosmetics[_target].Id = ((IProductModel)card.dataSource).GUID;
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
            IProductModel model = (IProductModel)_selected.dataSource;
            bool owned = _shop.IsOwned(_target, model);
            if (_shop.TryAcquire(_target, model))
            {
                _appliedProducts[_target].Id = model.GUID;
                if (owned)
                {
                    ApplyProduct();
                }
                else
                {
                    OwnProduct();
                }
            }

            UpdateButton();
        }

        private void RefreshOwnership()
        {
            foreach (string id in _models[_target].OwnedProductIds)
            {
                _view.OwnProduct(FindProductCard(id));
            }

            UpdateButton();
        }

        private void OwnProduct()
        {
            _view.OwnProduct(_selected);
            UpdateButton();
        }

        private void ApplyProduct()
        {
            _view.ApplyProductCard(_selected);
        }

        private void SelectAppliedProduct()
        {
            _selected = FindProductCard(_appliedProducts[_target].Id);
            ApplyProduct();
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
                IProductModel model = (IProductModel)_selected.dataSource;
                bool owned = _shop.IsOwned(_target, model);
                _view.Apply.text = owned ? "Apply" : "Exchange";
                _view.Apply.SetEnabled(owned
                    ? _appliedProducts[_target].Id != model.GUID
                    : _wallet.Balance >= model.Price);
            }
        }

        private VisualElement FindProductCard(string id)
        {
            return _view.ProductCards.Single(v => ((IProductModel)v.dataSource).GUID == id);
        }

        private void ReleaseProducts()
        {
            if (_productsHandle.IsValid())
            {
                Addressables.Release(_productsHandle);
                _productsHandle = default;
            }
        }
    }
}