using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Input.Contract;
using LeaseExtension.State;
using MessagePipe;
using R3;

namespace LeaseExtension.Gameplay.States
{
    [UsedImplicitly]
    internal class IdleState : PoolablePublishingState<IdleState, ICharacterContext, GameplayState>
    {
        private readonly ICharacterModel _characterModel;
        private readonly PunchState.Factory _punchFactory;
        private readonly ISubscriber<PunchRequested> _punchInput;
        private IDisposable _disposable;

        public IdleState(
            ICharacterContext context,
            PunchState.Factory punchFactory,
            ReactiveProperty<GameplayState> publisher,
            ISubscriber<PunchRequested> punchInput,
            ICharacterModel characterModel) : base(context, publisher, GameplayState.Idle)
        {
            _characterModel = characterModel;
            _punchFactory = punchFactory;
            _punchInput = punchInput;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _punchInput.Subscribe(Punch);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
            base.Dispose();
        }

        private void Punch(PunchRequested _)
        {
            Context.State = _punchFactory.Create(_characterModel.InitialPunchHeight);
        }
    }
}