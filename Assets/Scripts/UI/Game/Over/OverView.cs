using JetBrains.Annotations;
using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Over
{
    [UsedImplicitly]
    internal class OverView : View, IOverView
    {
        public Button Accept { get; private set; }

        public OverView(PanelRenderer panelRenderer) : base(panelRenderer, "over")
        {}

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            Accept = root.Q<Button>("over__accept-button");
        }
    }
}