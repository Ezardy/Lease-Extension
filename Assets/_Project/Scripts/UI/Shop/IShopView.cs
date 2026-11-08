using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    internal interface IShopView : IView
    {
        public Button Close { get; }
        public DropdownField CosmeticsTarget { get; }
        public Button Apply { get; }
        public ListView ProductList { get; }
    }
}