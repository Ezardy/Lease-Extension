using UnityEngine.UIElements;

namespace Aniki.UI {
	internal interface IGameViewModel {
		public StyleEnum<DisplayStyle>	GameDisplayStyle { get; }
		public uint						BarsPassed { get; }
		public uint						Earned { get; }
	}
}
