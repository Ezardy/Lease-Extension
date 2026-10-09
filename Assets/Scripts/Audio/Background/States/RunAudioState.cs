using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using R3;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class RunAudioState : ABackgroundAudioState<RunAudioState>
    {
        private readonly ReadOnlyReactiveProperty<CharacterState> _characterStateSubscriber;
        private readonly ResultAudioState.Factory _resultFactory;
        private IDisposable _disposable;

        public RunAudioState(
            IBackgroundAudioContext context,
            ReadOnlyReactiveProperty<CharacterState> characterStateSubscriber,
            ResultAudioState.Factory resultFactory) : base(context, "Run")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _resultFactory = resultFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(s =>
            {
                if (s == CharacterState.Over)
                {
                    Context.State = _resultFactory.Create();
                }
            });
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            base.Dispose();
        }
    }
}
