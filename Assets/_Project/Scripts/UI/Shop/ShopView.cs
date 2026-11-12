using System;
using System.Linq;
using System.Collections.Generic;
using Aniki.UI.Elements;
using JetBrains.Annotations;
using LeaseExtension.Shop.Contract;
using LeaseExtension.UI.Control;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopView : View, IShopView
    {
        private SelectableScrollView _productList;

        public Button Close { get; private set; }
        public EnumField CosmeticsTarget { get; private set; }
        public Button Apply { get; private set; }
        public event Action<IProductModel> OnProductSelected;
        public event Action OnProductSelectionCleared;

        public ShopView(PanelRenderer panelRenderer) : base(panelRenderer, "shop")
        {}

        public void SetProducts(IEnumerable<IProductModel> products)
        {
            _productList.SetItems(products, BindProduct);
        }

        public void RefreshProducts(IReadOnlyCollection<IProductModel> ownedProducts)
        {
            foreach (VisualElement element in _productList.Children())
            {
                IProductModel product = (IProductModel)element.dataSource;
                bool owned = ownedProducts.Contains(product);
                element.Q<IconLabel>("product__price").style.display = owned ? DisplayStyle.None : DisplayStyle.Flex;
                element.Q("product__preview").EnableInClassList("product__preview--not-acquired", !owned);
            }
        }

        public void SelectProduct(IProductModel product)
        {
            _productList.SelectItem(_productList.Children().Single(element => element.dataSource == product));
        }

        public void ClearProducts()
        {
            _productList.ClearItems();
        }

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            Close = root.Q<Button>("shop__close-button");
            CosmeticsTarget = root.Q<EnumField>("shop__cosmetics-target");
            Apply = root.Q<Button>("shop__apply-button");

            _productList = root.Q<SelectableScrollView>("shop__product-list");
            _productList.OnSelectionChanged += OnSelectionChanged;
            _productList.OnSelectionCleared += OnSelectionCleared;
        }

        private void OnSelectionChanged(VisualElement element)
        {
            OnProductSelected?.Invoke((IProductModel)element.dataSource);
        }

        private void OnSelectionCleared()
        {
            OnProductSelectionCleared?.Invoke();
        }

        private static void BindProduct(VisualElement element, IProductModel product)
        {
            element.dataSource = product;
        }
    }
}
