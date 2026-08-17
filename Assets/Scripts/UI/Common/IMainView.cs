using UnityEngine.UIElements;

namespace Aniki.UI {
	public interface IMainView : IView {
		public Button			StoreButton { get; }
		public VisualElement	EarnedGroup { get; }
	}
}
