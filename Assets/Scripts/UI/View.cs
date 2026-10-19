using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI
{
    internal abstract class View : IView
    {
        protected readonly string RootName;

        public VisualElement Root { get; private set; }

        protected View(PanelRenderer panelRenderer, string rootName)
        {
            RootName = rootName;
            panelRenderer.RegisterUIReloadCallback(OnGUIReload);
        }

        protected virtual void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            panelRenderer.UnregisterUIReloadCallback(OnGUIReload);
            Root = root.Q<VisualElement>(RootName);
        }
    }
}