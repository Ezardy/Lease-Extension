using System;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.State;
using MessagePipe;

namespace LeaseExtension.Gameplay.States
{
    internal class OverState : APoolablePublishingState<OverState, ICharacterContext, CharacterState>
    {
        private readonly ISubscriber<RestartRequested> _resetSubscriber;
        private readonly IdleState.Factory _idleFactory;
        private IDisposable _disposable;

        public OverState(
            ICharacterContext context,
            ISubscriber<RestartRequested> resetSubscriber,
            StatePublisher<CharacterState>.Factory publisherFactory,
            IdleState.Factory idleFactory) : base(
            context,
            publisherFactory.Create(CharacterState.Over))
        {
            _idleFactory = idleFactory;
            _resetSubscriber = resetSubscriber;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _resetSubscriber.Subscribe(_ => Context.State = _idleFactory.Create());
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            base.Dispose();
        }
    }
}
