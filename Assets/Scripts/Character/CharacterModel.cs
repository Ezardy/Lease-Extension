using R3;
using Unity.Properties;
using UnityEngine;

namespace Aniki.Character {
	[CreateAssetMenu(fileName = "CharacterModel", menuName = "Scriptable Objects/Character Model")]
	internal class CharacterModel : ScriptableObject, ICharacterModel {
		[SerializeField, DontCreateProperty] private SerializableReactiveProperty<float>	punchHeight;
		[SerializeField, DontCreateProperty] private SerializableReactiveProperty<float>	initialPunchHeight;
		[SerializeField, DontCreateProperty] private SerializableReactiveProperty<float>	punchPeriod;

		private readonly SerializableReactiveProperty<CharacterState>	state = new(CharacterState.IDLE);

		public float	PunchHeight => punchHeight.Value;
		public float	InitialPunchHeight => initialPunchHeight.Value;
		public float	PunchPeriod => punchPeriod.Value;

		[CreateProperty]
		public CharacterState	State {
			get => state.Value;
			set => state.Value = value;
		}

		public Observable<float>			PunchHeightChanged => punchHeight;
		public Observable<float>			InitialPunchHeightChanged => initialPunchHeight;
		public Observable<float>			PunchPeriodChanged => punchPeriod;

		public Observable<CharacterState>	StateChanged => state;
	}
}
