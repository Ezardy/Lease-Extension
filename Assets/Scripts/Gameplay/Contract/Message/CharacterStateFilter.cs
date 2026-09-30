using Aniki.Common;

namespace Aniki.Character {
	public class CharacterStateFilter : EqualityFilter<CharacterState> {
		public static readonly CharacterStateFilter	Idle = new(CharacterState.IDLE);
		public static readonly CharacterStateFilter	Wait = new(CharacterState.WAIT);
		public static readonly CharacterStateFilter	Punch = new(CharacterState.PUNCH);
		public static readonly CharacterStateFilter	Fall = new(CharacterState.FALL);
		public static readonly CharacterStateFilter	Over = new(CharacterState.OVER);

		public CharacterStateFilter(CharacterState state)
			: base(state) { }
	}
}
