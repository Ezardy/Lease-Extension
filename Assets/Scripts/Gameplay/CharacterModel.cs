using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using R3;
using Unity.Properties;
using UnityEngine;

namespace LeaseExtension.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterModel", menuName = "Scriptable Objects/Character Model")]
    internal class CharacterModel : ScriptableObject, ICharacterModel
    {
        [SerializeField, DontCreateProperty]
        private SerializableReactiveProperty<float> _punchHeight;
        [SerializeField, DontCreateProperty]
        private SerializableReactiveProperty<float> _initialPunchHeight;
        [SerializeField, DontCreateProperty]
        private SerializableReactiveProperty<float> _punchPeriod;
        private readonly SerializableReactiveProperty<CharacterState> _state = new(CharacterState.Idle);

        public float PunchHeight => _punchHeight.Value;
        public float InitialPunchHeight => _initialPunchHeight.Value;
        public float PunchPeriod => _punchPeriod.Value;

        [CreateProperty]
        public CharacterState State
        {
            get => _state.Value;
            set => _state.Value = value;
        }
        public Observable<float> PunchHeightChanged => _punchHeight;
        public Observable<float> InitialPunchHeightChanged => _initialPunchHeight;
        public Observable<float> PunchPeriodChanged => _punchPeriod;
        public Observable<CharacterState> StateChanged => _state;
    }
}
