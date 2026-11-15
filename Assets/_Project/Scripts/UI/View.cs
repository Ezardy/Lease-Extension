using LeaseExtension.UI.Contract;
using UnityEngine.UIElements;

namespace LeaseExtension.UI
{
    internal class View : IView
    {
        private readonly string _rootName;

        public VisualElement Root { get; private set; }

        public View(PanelRenderer panelRenderer, string rootName)
        {
            _rootName = rootName;
            panelRenderer.RegisterUIReloadCallback(OnGUIReload);
        }

        protected virtual void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            panelRenderer.UnregisterUIReloadCallback(OnGUIReload);
            Root = root.Q<VisualElement>(_rootName);
        }
    }
}