using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Gameplay.States;
using MessagePipe;
using Zenject;
using Context = LeaseExtension.State.Context;

namespace LeaseExtension.Gameplay
{
    [UsedImplicitly]
    internal class CharacterStateMachine : Context, ICharacterContext, IInitializable, IFixedTickable
    {
        private readonly IdleState.Factory _idleFactory;

        public CharacterStateMachine(
            ICharacterModel characterModel,
            IdleState.Factory idleFactory,
            ISubscriber<GameplayState> stateSubscriber)
        {
            _idleFactory = idleFactory;
        }

        public void Initialize()
        {
            State = _idleFactory.Create();
        }

        public void FixedTick()
        {
            State.Update();
        }
    }
}