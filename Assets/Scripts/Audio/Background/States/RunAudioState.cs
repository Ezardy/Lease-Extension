using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using MessagePipe;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class RunAudioState : ABackgroundAudioState<RunAudioState>
    {
        private readonly ISubscriber<CharacterState> _characterStateSubscriber;
        private readonly ResultAudioState.Factory _resultFactory;
        private IDisposable _disposable;

        public RunAudioState(
            IBackgroundAudioContext context,
            ISubscriber<CharacterState> characterStateSubscriber,
            ResultAudioState.Factory resultFactory) : base(context, "Run")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _resultFactory = resultFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(
                _ => Context.State = _resultFactory.Create(),
                CharacterStateFilter.Over);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            base.Dispose();
        }
    }
}
