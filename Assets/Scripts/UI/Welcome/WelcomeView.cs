using UnityEngine.UIElements;

namespace Aniki.UI {
	internal class WelcomeView : AView, IWelcomeView {
		public WelcomeView(PanelRenderer panelRenderer) : base(panelRenderer, "welcome") { }
	}
}
