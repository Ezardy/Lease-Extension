using System;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.State;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using R3;
using UnityEngine;

namespace LeaseExtension.Gameplay.States
{
    internal class FallState : APoolablePublishingState<FallState, ICharacterContext, CharacterState>
    {
        private readonly ISubscriber<FloorCollided> _subscriber;
        private readonly OverState.Factory _overStateFactory;
        private readonly Rigidbody2D _rigidbody;
        private IDisposable _subscription;

        public FallState(
            ICharacterContext context,
            StatePublisher<CharacterState>.Factory publisherFactory,
            Rigidbody2D rigidbody,
            ISubscriber<FloorCollided> subscriber,
            OverState.Factory overStateFactory) : base(
            context,
            publisherFactory.Create(CharacterState.Fall))
        {
            _rigidbody = rigidbody;
            _subscriber = subscriber;
            _overStateFactory = overStateFactory;
        }

        public override void Start()
        {
            base.Start();
            _subscription = _subscriber.Subscribe(_ => Context.State = _overStateFactory.Create());
            _rigidbody.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
        }

        public override void Dispose()
        {
            _subscription.Dispose();
            _subscription = null;
            base.Dispose();
        }
    }
}
