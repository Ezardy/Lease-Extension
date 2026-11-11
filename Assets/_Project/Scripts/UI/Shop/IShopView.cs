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
        public event Action<IProductModel> OnProductSelected;
        public event Action OnProductSelectionCleared;
        public void SetProducts(IEnumerable<IProductModel> products);
        public void RefreshProducts(IReadOnlyCollection<IProductModel> ownedProducts);
        public void SelectProduct(IProductModel product);
        public void ClearProducts();
    }
}
