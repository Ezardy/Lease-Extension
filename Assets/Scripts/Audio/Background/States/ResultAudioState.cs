using System;
using LeaseExtension.Gameplay.Contract.Message;
using MessagePipe;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Audio.Background.States
{
    [MovedFrom("Aniki.Audio")]
    internal class ResultAudioState : ABackgroundAudioState<ResultAudioState>
    {
        private readonly ISubscriber<CharacterState> _characterStateSubscriber;
        private readonly IdleAudioState.Factory _idleFactory;
        private IDisposable _disposable;

        public ResultAudioState(
            IBackgroundAudioContext context,
            ISubscriber<CharacterState> characterStateSubscriber,
            IdleAudioState.Factory idleFactory) : base(context, "Result")
        {
            _characterStateSubscriber = characterStateSubscriber;
            _idleFactory = idleFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _characterStateSubscriber.Subscribe(
                _ => Context.State = _idleFactory.Create(),
                CharacterStateFilter.Idle);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            base.Dispose();
        }
    }
}
