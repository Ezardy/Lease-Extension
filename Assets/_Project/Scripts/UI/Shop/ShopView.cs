using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopView : View, IShopView
    {
        public Button Close { get; private set; }
        public EnumField CosmeticsTarget { get; private set; }
        public Button Apply { get; private set; }
        public ListView ProductList { get; private set; }

        public ShopView(PanelRenderer panelRenderer) : base(panelRenderer, "shop")
        {}

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            Close = root.Q<Button>("shop__close-button");
            CosmeticsTarget = root.Q<EnumField>("shop__cosmetics-target");
            Apply = root.Q<Button>("shop__apply-button");
            ProductList = root.Q<ListView>("shop__product-list");
        }
    }
}