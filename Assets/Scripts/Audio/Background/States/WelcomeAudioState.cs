using System;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using Zenject;

namespace LeaseExtension.Audio.Background.States
{
    internal class WelcomeAudioState : AState<IBackgroundAudioContext>
    {
        private readonly IDisposable _disposable;

        public WelcomeAudioState(
            IBackgroundAudioContext context,
            ISubscriber<FocusedScene> sceneSubscriber,
            IdleAudioState.Factory idleFactory) : base(context)
        {
            _disposable = sceneSubscriber.Subscribe(_ => context.State = idleFactory.Create(), FocusedSceneFilter.Main);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
        }

        public class Factory : PlaceholderFactory<WelcomeAudioState>
        {
        }
    }
}
