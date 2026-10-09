using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Gameplay.States;
using LeaseExtension.State;
using MessagePipe;
using R3;
using Zenject;

namespace LeaseExtension.Gameplay
{
    [UsedImplicitly]
    internal class CharacterStateMachine : AContext, ICharacterContext, IInitializable, IFixedTickable
    {
        private readonly IdleState.Factory _idleFactory;

        public CharacterStateMachine(
            ICharacterModel characterModel,
            IdleState.Factory idleFactory,
            ISubscriber<CharacterState> stateSubscriber)
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
