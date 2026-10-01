using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI
{
    internal abstract class AView : IView
    {
        protected readonly string RootName;

        public VisualElement Root { get; private set; }

        protected AView(PanelRenderer panelRenderer, string rootName)
        {
            this.RootName = rootName;
            panelRenderer.RegisterUIReloadCallback(OnGUIReload);
        }

        protected virtual void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            panelRenderer.UnregisterUIReloadCallback(OnGUIReload);
            this.Root = root.Q<VisualElement>(RootName);
        }
    }
}
