using LeaseExtension.Common.Utilities;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Gameplay.Contract.Message
{
    [MovedFrom("Aniki.Character")]
    public class CharacterStateFilter : EqualityFilter<CharacterState>
    {
        public static readonly CharacterStateFilter Idle = new(CharacterState.Idle);
        public static readonly CharacterStateFilter Wait = new(CharacterState.Wait);
        public static readonly CharacterStateFilter Punch = new(CharacterState.Punch);
        public static readonly CharacterStateFilter Fall = new(CharacterState.Fall);
        public static readonly CharacterStateFilter Over = new(CharacterState.Over);

        public CharacterStateFilter(CharacterState state) : base(state)
        {
        }
    }
}
