using UnityEngine.UIElements;

namespace Aniki.UI {
	public interface IMainViewModel {
		public StyleEnum<DisplayStyle>	MainDisplayStyle { get; }
		public uint						Record { get; }
	}
}
