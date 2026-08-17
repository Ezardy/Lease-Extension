using R3;

namespace Aniki.Character {
	public interface ICharacterModel {
		public float			PunchHeight { get; }
		public float			InitialPunchHeight { get; }
		public float			PunchPeriod { get; }
		public CharacterState	State { get; set; }

		public Observable<float>			PunchHeightChanged { get; }
		public Observable<float>			InitialPunchHeightChanged { get; }
		public Observable<float>			PunchPeriodChanged { get; }
		public Observable<CharacterState>	StateChanged { get; }
	}
}
