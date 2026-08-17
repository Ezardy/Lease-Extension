using System;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal abstract class AView : IView, IDisposable {
		private readonly PanelRenderer	panelRenderer;
		protected readonly string		rootName;

		public VisualElement	Root => root;

		private VisualElement	root;

		public AView(PanelRenderer panelRenderer, string rootName) {
			this.panelRenderer = panelRenderer;
			this.rootName = rootName;
			panelRenderer.RegisterUIReloadCallback(OnGUIReload);
		}

		protected virtual void	OnGUIReload(PanelRenderer panelRenderer, VisualElement root) {
			this.root = root.Q<VisualElement>(rootName);
		}

		public void	Dispose() {
			panelRenderer.UnregisterUIReloadCallback(OnGUIReload);
		}
	}
}
