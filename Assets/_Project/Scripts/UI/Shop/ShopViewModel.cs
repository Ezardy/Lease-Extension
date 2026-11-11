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
        private readonly IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel> _applied;
        private readonly IWalletModel _wallet;
        private readonly IShop _shop;
        private readonly IShopPreviewSpawner _preview;
        private readonly List<IDisposable> _subscriptions = new();
        private readonly CancellationTokenSource _viewInitialization = new();
        private AsyncOperationHandle<IList<IProductModel>> _productsHandle;
        private CosmeticsTarget _target;
        private IProductModel _selected;

        public ShopViewModel(
            IShopView view,
            IPublisher<FocusedScene> sceneRequestPublisher,
            IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> models,
            IReadOnlyDictionary<CosmeticsTarget, IAppliedCosmeticsModel> applied,
            IWalletModel wallet,
            IShop shop,
            IShopPreviewSpawner preview)
        {
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            _models = models;
            _applied = applied;
            _wallet = wallet;
            _shop = shop;
            _preview = preview;
            InitializeView().Forget();
        }

        public void Dispose()
        {
            _viewInitialization.Cancel();
            _viewInitialization.Dispose();
            foreach (IDisposable subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            _preview.Clear();
            if (_view.Root != null)
            {
                _view.Close.clicked -= OnCloseClicked;
                _view.Apply.clicked -= OnApplyClicked;
                _view.OnProductSelected -= OnProductSelected;
                _view.CosmeticsTarget.UnregisterValueChangedCallback(OnCosmeticsTargetChanged);
                _view.ClearProducts();
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
            _view.CosmeticsTarget.RegisterValueChangedCallback(OnCosmeticsTargetChanged);
            _subscriptions.Add(_wallet.BalanceChanged.Subscribe(_ => UpdateButton()));
            foreach (KeyValuePair<CosmeticsTarget, ITargetProductListModel> entry in _models)
            {
                CosmeticsTarget target = entry.Key;
                _subscriptions.Add(entry.Value.ProductOwned.Subscribe(_ => RefreshOwnership()));
                _subscriptions.Add(_applied[target].IdChanged.Subscribe(id => OnAppliedChanged(target, id)));
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
            OnProductSelected(null);
            _view.ClearProducts();
            ReleaseProducts();
            IReadOnlyCollection<AssetReference> products = _models[target].Products;
            if (products.Count > 0)
            {
                IEnumerable<object> keys = products.Select(product => product.RuntimeKey);
                _productsHandle = Addressables.LoadAssetsAsync<IProductModel>(keys, null, Addressables.MergeMode.Union);
                _productsHandle.Completed += OnProductsLoaded;
            }
        }

        private void OnProductsLoaded(AsyncOperationHandle<IList<IProductModel>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _view.SetProducts(handle.Result, _models[_target].OwnedProductIds, _applied[_target].Id);
            }
        }

        private void OnProductSelected(IProductModel product)
        {
            _selected = product;
            _preview.Show(_target, product);
            UpdateButton();
        }

        private void OnApplyClicked()
        {
            _shop.ExchangeOrApply(_target, _selected);
            UpdateButton();
        }

        private void RefreshOwnership()
        {
            _view.RefreshProducts(_models[_target].OwnedProductIds);
            UpdateButton();
        }

        private void OnAppliedChanged(CosmeticsTarget target, string id)
        {
            if (target == _target && _productsHandle.IsValid()
                && _productsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _view.SetProducts(_productsHandle.Result, _models[target].OwnedProductIds, id);
            }

            UpdateButton();
        }

        private void UpdateButton()
        {
            bool owned = _selected != null && _selected.IsOwnedBy(_models[_target]);
            _view.Apply.text = _selected == null || owned ? "Apply" : "Exchange";
            _view.Apply.SetEnabled(_selected != null && (owned
                ? !_selected.IsIdentifiedBy(_applied[_target].Id)
                : _wallet.Balance >= _selected.Price));
        }

        private void ReleaseProducts()
        {
            if (_productsHandle.IsValid())
            {
                _productsHandle.Completed -= OnProductsLoaded;
                Addressables.Release(_productsHandle);
                _productsHandle = default;
            }
        }
    }
}
