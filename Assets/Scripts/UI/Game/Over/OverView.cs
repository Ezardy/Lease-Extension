using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Over
{
    internal class OverView : AView, IOverView
    {
        public Button AcceptButton { get; private set; }

        public OverView(PanelRenderer panelRenderer) : base(panelRenderer, "over")
        {
        }

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            AcceptButton = root.Q<Button>("over__accept-button");
        }
    }
}
