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
        public void SetProducts(IEnumerable<IProductModel> products);
        public void ClearProducts();
    }
}
