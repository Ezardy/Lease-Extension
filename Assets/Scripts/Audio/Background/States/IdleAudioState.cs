using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using R3;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class IdleAudioState : ABackgroundAudioState<IdleAudioState>
    {
        private readonly ReadOnlyReactiveProperty<CharacterState> _characterStateSubscriber;
        private readonly RunAudioState.Factory _runFactory;
        private IDisposable _disposable;

        public IdleAudioState(
            IBackgroundAudioContext context,
            ReadOnlyReactiveProperty<CharacterState> characterStateSubscriber,
            RunAudioState.Factory runFactory) : base(context, "Idle")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _runFactory = runFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(s =>
            {
                if (s == CharacterState.Punch)
                {
                    //_disposable.Dispose();
                    //_disposable = null;
                    Context.State = _runFactory.Create();
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
