using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Shop
{
    [UsedImplicitly]
    internal class ShopView : View, IShopView
    {
        public Button Close { get; private set; }

        public ShopView(PanelRenderer panelRenderer) : base(panelRenderer, "shop")
        {}

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            Close = root.Q<Button>("shop__close-button");
        }
    }
}