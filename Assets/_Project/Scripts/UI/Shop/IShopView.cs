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
        public IEnumerable<VisualElement> Cards { get; }
        public event Action<VisualElement> OnProductSelected;
        public event Action OnProductDeselected;
        public void SetProducts(IEnumerable<IProductModel> products);
        public void OwnProduct(VisualElement card);
        public void SelectProduct(IProductModel product);
        public void ClearProducts();
    }
}
