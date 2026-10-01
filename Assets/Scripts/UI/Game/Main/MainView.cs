using JetBrains.Annotations;
using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    [UsedImplicitly]
    internal class MainView : AView, IMainView
    {
        public Button StoreButton { get; private set; }
        public VisualElement EarnedGroup { get; private set; }

        public MainView(PanelRenderer panelRenderer) : base(panelRenderer, "main")
        {
        }

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            StoreButton = root.Q<Button>("store-button");
            EarnedGroup = root.Q<VisualElement>("game-stats__earned-group");
        }
    }
}
