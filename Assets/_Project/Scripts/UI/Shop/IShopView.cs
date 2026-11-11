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
        public void SetProducts(IEnumerable<IProductModel> products, IReadOnlyCollection<string> ownedProductIds, string selectedId);
        public void RefreshProducts(IReadOnlyCollection<string> ownedProductIds);
        public void ClearProducts();
    }
}
