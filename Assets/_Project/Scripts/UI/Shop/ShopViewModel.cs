using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aniki.UI.Elements;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.Shop.Contract;
using MessagePipe;
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
        private readonly CancellationTokenSource _viewInitialization = new();
        private AsyncOperationHandle<IList<IProductModel>> _productsHandle;

        public ShopViewModel(
            IShopView view,
            IPublisher<FocusedScene> sceneRequestPublisher,
            IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> models)
        {
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            _models = models;
            InitializeView().Forget();
        }

        public void Dispose()
        {
            _viewInitialization.Cancel();
            _viewInitialization.Dispose();
            if (_view.Root != null)
            {
                _view.Close.clicked -= OnCloseClicked;
                _view.CosmeticsTarget.UnregisterValueChangedCallback(OnCosmeticsTargetChanged);
                _view.ProductList.itemsSource = null;
                _view.ProductList.bindItem = null;
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
            _view.CosmeticsTarget.RegisterValueChangedCallback(OnCosmeticsTargetChanged);
            _view.ProductList.bindItem = BindProduct;
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
            _view.ProductList.ClearSelection();
            _view.ProductList.itemsSource = null;
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
                _view.ProductList.itemsSource = handle.Result.ToArray();
            }
        }

        private void BindProduct(VisualElement element, int index)
        {
            IProductModel product = (IProductModel)_view.ProductList.itemsSource[index];
            Image back = element.Q<Image>("product__preview-back");
            back.image = null;
            back.sprite = product.Back;
            Image front = element.Q<Image>("product__preview-front");
            front.image = null;
            front.sprite = product.Front;
            element.Q<IconLabel>("product__price").Label = product.Price.ToString();
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
