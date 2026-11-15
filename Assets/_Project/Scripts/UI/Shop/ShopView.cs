using System;
using System.Collections.Generic;
using Aniki.UI.Elements;
using JetBrains.Annotations;
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
        public IEnumerable<VisualElement> ProductCards => _productList.Children();

        public event Action<VisualElement> OnProductSelected;
        public event Action OnProductDeselected;

        public ShopView(PanelRenderer panelRenderer) : base(panelRenderer, "shop")
        {}

        public void SetProducts<T>(IEnumerable<T> products, Action<VisualElement, T> bindFunc)
        {
            _productList.SetItems(products, bindFunc);
        }

        public void OwnProduct(VisualElement card)
        {
            card.Q<IconLabel>("product__price").style.display = DisplayStyle.None;
            card.Q("product__preview").EnableInClassList("product__preview--not-acquired", false);
        }

        public void ApplyProductCard(VisualElement product)
        {
            _productList.SelectItem(product);
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
            OnProductSelected?.Invoke(element);
        }

        private void OnSelectionCleared()
        {
            OnProductDeselected?.Invoke();
        }
    }
}