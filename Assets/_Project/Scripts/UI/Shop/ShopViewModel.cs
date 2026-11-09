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
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopViewModel : IShopViewModel, IDisposable
    {
        private const string PRODUCT_CARD_CLASS = "shop__product-card";
        private const string SELECTED_CARD_CLASS = "shop__product-card--selected";

        private readonly IShopView _view;
        private readonly IPublisher<FocusedScene> _sceneRequestPublisher;
        private readonly IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> _models;
        private readonly VisualTreeAsset _productTemplate;
        private readonly CancellationTokenSource _viewInitialization = new();
        private AsyncOperationHandle<IList<IProductModel>> _productsHandle;
        private VisualElement _selectedCard;

        public ShopViewModel(
            IShopView view,
            IPublisher<FocusedScene> sceneRequestPublisher,
            IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> models,
            VisualTreeAsset productTemplate)
        {
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            _models = models;
            _productTemplate = productTemplate;
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
                ClearProductList();
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
            ClearProductList();
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
                foreach (IProductModel product in handle.Result)
                {
                    TemplateContainer card = _productTemplate.Instantiate();
                    card.AddToClassList(PRODUCT_CARD_CLASS);
                    card.focusable = true;
                    card.AddManipulator(new Clickable(() => SelectCard(card)));
                    card.RegisterCallback<NavigationSubmitEvent>(_ => SelectCard(card));
                    BindProduct(card, product);
                    _view.ProductList.Add(card);
                }
            }
        }

        private static void BindProduct(VisualElement element, IProductModel product)
        {
            Image back = element.Q<Image>("product__preview-back");
            back.image = product.Back ? product.Back.texture : null;
            Image front = element.Q<Image>("product__preview-front");
            front.image = product.Front ? product.Front.texture : null;
            element.Q<IconLabel>("product__price").Label = product.Price.ToString();
        }

        private void SelectCard(VisualElement card)
        {
            _selectedCard?.RemoveFromClassList(SELECTED_CARD_CLASS);
            _selectedCard = card;
            _selectedCard.AddToClassList(SELECTED_CARD_CLASS);
            _view.ProductList.ScrollTo(card);
        }

        private void ClearProductList()
        {
            _selectedCard = null;
            _view.ProductList.Clear();
            _view.ProductList.scrollOffset = Vector2.zero;
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
