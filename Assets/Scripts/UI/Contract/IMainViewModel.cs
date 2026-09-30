using UnityEngine.UIElements;

namespace Aniki.UI {
	internal interface IMainViewModel {
		public StyleEnum<DisplayStyle>	MainDisplayStyle { get; }
		public uint						Record { get; }
	}
}
