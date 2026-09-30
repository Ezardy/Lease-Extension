using System;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Gameplay.States;
using LeaseExtension.State;
using MessagePipe;
using R3;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.Gameplay
{
    [MovedFrom("Aniki.Character")]
    internal class CharacterStateMachine : AContext, ICharacterContext, IInitializable, IFixedTickable
    {
        private readonly IdleState.Factory _idleFactory;
        private readonly IDisposable _disposable;

        public CharacterStateMachine(
            ICharacterModel characterModel,
            IdleState.Factory idleFactory,
            ISubscriber<CharacterState> stateSubscriber)
        {
            _idleFactory = idleFactory;
            _disposable = stateSubscriber.Subscribe(s => characterModel.State = s);
        }

        public void Initialize()
        {
            State = _idleFactory.Create();
        }

        public void FixedTick()
        {
            State.Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposable.Dispose();
        }
    }
}
