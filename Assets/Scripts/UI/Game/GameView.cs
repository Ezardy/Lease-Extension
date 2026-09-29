using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class GameView : AView, IGameView {
		private VisualElement	effects;

		public void	AddEffect(string id, float duration, Texture texture) {
			throw new System.NotImplementedException();
		}

		public GameView(PanelRenderer panelRenderer) : base(panelRenderer, "game") { }

		protected override void	OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version) {
			base.OnGUIReload(panelRenderer, root, version);
			effects = root.Q<VisualElement>("effects");
		}
	}
}
