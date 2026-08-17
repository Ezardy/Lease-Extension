using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class OverView : AView, IOverView {
		private Button			acceptButton;

		public OverView(PanelRenderer panelRenderer) : base(panelRenderer, "game-over") { }

		public Button AcceptButton => acceptButton;

		protected override void	OnGUIReload(PanelRenderer panelRenderer, VisualElement root) {
			base.OnGUIReload(panelRenderer, root);
			acceptButton = root.Q<Button>("game-over__accept-button");
		}
	}
}
