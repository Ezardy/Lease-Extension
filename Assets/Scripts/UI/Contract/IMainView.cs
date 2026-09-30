using UnityEngine.UIElements;

namespace Aniki.UI {
	internal interface IMainView : IView {
		public Button			StoreButton { get; }
		public VisualElement	EarnedGroup { get; }
	}
}
