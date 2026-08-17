using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class MainView : AView, IMainView {
		private Button			storeButton;
		private VisualElement	earnedGroup;

		public MainView(PanelRenderer panelRenderer) : base(panelRenderer, "main") { }

		public Button			StoreButton => storeButton;
		public VisualElement	EarnedGroup => earnedGroup;

		protected override void	OnGUIReload(PanelRenderer panelRenderer, VisualElement root) {
			base.OnGUIReload(panelRenderer, root);
			storeButton = root.Q<Button>("store-button");
			earnedGroup = root.Q<VisualElement>("game-stats__earned-group");
		}
	}
}
