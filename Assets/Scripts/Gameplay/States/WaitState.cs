using System;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Input.Contract;
using LeaseExtension.State;
using LeaseExtension.World.Contract;
using MessagePipe;
using R3;

namespace LeaseExtension.Gameplay.States
{
    internal class WaitState : APoolablePublishingState<WaitState, ICharacterContext, CharacterState>
    {
        private readonly CollisionCheckStateBase _stateBase;
        private readonly PunchState.Factory _punchFactory;
        private readonly ICharacterModel _characterModel;
        private readonly INoPunchZone _noPunchZone;
        private readonly ISubscriber<PunchRequested> _punchInput;
        private IDisposable _disposable;

        public WaitState(
            ICharacterContext context,
            StatePublisher<CharacterState>.Factory publisherFactory,
            CollisionCheckStateBase stateBase,
            PunchState.Factory punchFactory,
            ISubscriber<PunchRequested> punchInput,
            INoPunchZone noPunchZone,
            ICharacterModel characterModel) : base(
            context,
            publisherFactory.Create(CharacterState.Wait))
        {
            _stateBase = stateBase;
            _punchInput = punchInput;
            _punchFactory = punchFactory;
            _characterModel = characterModel;
            _noPunchZone = noPunchZone;
        }

        public override void Start()
        {
            base.Start();
            _stateBase.Start();
            _disposable = _punchInput.Subscribe(Punch);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
            _stateBase.Dispose();
            base.Dispose();
        }

        private void Punch(PunchRequested _)
        {
            if (!_noPunchZone.InZone)
                Context.State = _punchFactory.Create(_characterModel.PunchHeight);
        }
    }
}
