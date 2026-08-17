using UnityEngine.UIElements;

namespace Aniki.UI {
	public interface IOverViewModel {
		public StyleEnum<DisplayStyle>	OverDisplayStyle { get; }
		public StyleEnum<DisplayStyle>	NewRecordDisplayStyle { get; }
		public uint						BarsPassed { get; }
		public uint						Earned { get; }
	}
}
