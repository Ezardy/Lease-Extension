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

        public ShopView(PanelRenderer panelRenderer) : base(panelRenderer, "shop")
        {}

        public void SetProducts(IEnumerable<IProductModel> products, IReadOnlyCollection<string> ownedProductIds, string selectedId)
        {
            _productList.SetItems(products, BindProduct);
            RefreshProducts(ownedProductIds);
            VisualElement selected = _productList.Children().FirstOrDefault(element =>
                element.userData is IProductModel product && product.IsIdentifiedBy(selectedId));
            _productList.SelectItem(selected);
        }

        public void RefreshProducts(IReadOnlyCollection<string> ownedProductIds)
        {
            foreach (VisualElement element in _productList.Children())
            {
                IProductModel product = (IProductModel)element.userData;
                bool owned = ownedProductIds.Any(product.IsIdentifiedBy);
                element.Q<IconLabel>("product__price").style.display = owned ? DisplayStyle.None : DisplayStyle.Flex;
                element.Q("product__preview").EnableInClassList("product__preview--not-acquired", !owned);
            }
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
            if (_productList != null)
            {
                _productList.OnSelectionChanged -= OnSelectionChanged;
            }

            _productList = root.Q<SelectableScrollView>("shop__product-list");
            _productList.OnSelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(VisualElement element)
        {
            OnProductSelected?.Invoke(element?.userData as IProductModel);
        }

        private static void BindProduct(VisualElement element, IProductModel product)
        {
            element.userData = product;
            Image back = element.Q<Image>("product__preview-back");
            back.image = product.Back ? product.Back.texture : null;
            Image front = element.Q<Image>("product__preview-front");
            front.image = product.Front ? product.Front.texture : null;
            element.Q<IconLabel>("product__price").Label = product.Price.ToString();
            element.Q<Label>("product__label").text = product.Label;
        }
    }
}
