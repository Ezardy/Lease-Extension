using JetBrains.Annotations;
using UnityEngine;
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
            ProductList.virtualizationMethod = CollectionVirtualizationMethod.FixedHeight;
            ProductList.makeItem = CreateProductCard;
            VisualElement viewport = ProductList.Q<ScrollView>().contentViewport;
            viewport.style.alignSelf = Align.Stretch;
            viewport.RegisterCallback<GeometryChangedEvent>(OnProductViewportChanged);
        }

        private VisualElement CreateProductCard()
        {
            TemplateContainer card = ProductList.itemTemplate.Instantiate();
            card.style.flexGrow = 1;
            card.Q("product").style.alignSelf = Align.FlexStart;
            return card;
        }

        private void OnProductViewportChanged(GeometryChangedEvent evt)
        {
            VisualElement viewport = (VisualElement)evt.currentTarget;
            float height = viewport.contentRect.height;
            if (height > 0f && !Mathf.Approximately(ProductList.fixedItemHeight, height))
            {
                ProductList.fixedItemHeight = height;
                ProductList.Rebuild();
            }
        }
    }
}
