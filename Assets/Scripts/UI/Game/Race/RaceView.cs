using LeaseExtension.UI.Contract;
using UnityEngine;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Race
{
    internal class RaceView : AView, IRaceView
    {
        private VisualElement _effects;

        public RaceView(PanelRenderer panelRenderer) : base(panelRenderer, "race")
        {
        }

        public void AddEffect(string id, float duration, Texture texture)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            base.OnGUIReload(panelRenderer, root, version);
            _effects = root.Q<VisualElement>("effects");
        }
    }
}
