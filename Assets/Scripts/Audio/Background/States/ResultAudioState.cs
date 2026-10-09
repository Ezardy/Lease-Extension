using System;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using R3;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class ResultAudioState : ABackgroundAudioState<ResultAudioState>
    {
        private readonly Observable<CharacterState> _characterStateSubscriber;
        private readonly IdleAudioState.Factory _idleFactory;
        private IDisposable _disposable;

        public ResultAudioState(
            IBackgroundAudioContext context,
            Observable<CharacterState> characterStateSubscriber,
            IdleAudioState.Factory idleFactory) : base(context, "Result")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _idleFactory = idleFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(s =>
                {
                    if (s == CharacterState.Idle)
                    {
                        Context.State = _idleFactory.Create();
                    }
                }
            );
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            base.Dispose();
        }
    }
}