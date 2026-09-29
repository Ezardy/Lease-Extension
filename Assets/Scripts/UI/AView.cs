using System;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal abstract class AView : IView  {
		protected readonly string		rootName;

		public VisualElement	Root => root;

		private VisualElement	root;

		protected AView(PanelRenderer panelRenderer, string rootName) {
			this.rootName = rootName;
			panelRenderer.RegisterUIReloadCallback(OnGUIReload);
		}

		protected virtual void	OnGUIReload(PanelRenderer panelRenderer, VisualElement root, int version) {
			panelRenderer.UnregisterUIReloadCallback(OnGUIReload);
			this.root = root.Q<VisualElement>(rootName);
		}
	}
}
