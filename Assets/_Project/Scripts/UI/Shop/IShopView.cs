using System;
using System.Collections.Generic;
using LeaseExtension.Shop.Contract;
using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    internal interface IShopView : IView
    {
        public Button Close { get; }
        public EnumField CosmeticsTarget { get; }
        public Button Apply { get; }
        public IEnumerable<VisualElement> ProductCards { get; }
        public event Action<VisualElement> OnProductSelected;
        public event Action OnProductDeselected;
        public void SetProducts<T>(IEnumerable<T> products, Action<VisualElement, T> bindFunc);
        public void OwnProduct(VisualElement card);
        public void ApplyProductCard(VisualElement card);
        public void ClearProducts();
    }
}
