using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using MessagePipe;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class IdleAudioState : ABackgroundAudioState<IdleAudioState>
    {
        private readonly ISubscriber<CharacterState> _characterStateSubscriber;
        private readonly RunAudioState.Factory _runFactory;
        private IDisposable _disposable;

        public IdleAudioState(
            IBackgroundAudioContext context,
            ISubscriber<CharacterState> characterStateSubscriber,
            RunAudioState.Factory runFactory) : base(context, "Idle")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _runFactory = runFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(_ =>
            {
                _disposable.Dispose();
                _disposable = null;
                Context.State = _runFactory.Create();
            }, CharacterStateFilter.Punch);
        }

        public override void Dispose()
        {
            _disposable?.Dispose();
            base.Dispose();
        }
    }
}
