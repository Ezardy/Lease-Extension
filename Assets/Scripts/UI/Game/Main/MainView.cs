using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    [UsedImplicitly]
    internal class MainView : View, IMainView
    {
        public Button Shop { get; private set; }
        public VisualElement EarnedGroup { get; private set; }

        public MainView(PanelRenderer panelRenderer) : base(panelRenderer, "main")
        {}

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            Shop = root.Q<Button>("shop-button");
            EarnedGroup = root.Q<VisualElement>("game-stats__earned-group");
        }
    }
}