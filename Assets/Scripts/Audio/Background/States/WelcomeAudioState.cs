using System;
using JetBrains.Annotations;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using R3;
using Zenject;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class WelcomeAudioState : AState<IBackgroundAudioContext>
    {
        private readonly IDisposable _disposable;

        public WelcomeAudioState(
            IBackgroundAudioContext context,
            ReadOnlyReactiveProperty<FocusedScene> sceneSubscriber,
            IdleAudioState.Factory idleFactory) : base(context)
        {
            _disposable = sceneSubscriber.Subscribe(s =>
            {
                if (s == FocusedScene.Main)
                {
                    context.State = idleFactory.Create();
                }
            });
        }

        public override void Dispose()
        {
            _disposable.Dispose();
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<WelcomeAudioState>
        {
        }
    }
}
