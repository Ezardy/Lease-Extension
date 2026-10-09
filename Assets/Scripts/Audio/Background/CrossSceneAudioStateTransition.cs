using System;
using JetBrains.Annotations;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State.Contract;
using R3;
using Zenject;

namespace LeaseExtension.Audio.Background
{
    [UsedImplicitly]
    internal class CrossSceneAudioStateTransition<TState, TFactory> : IDisposable where TState : IState where TFactory : IFactory<TState>
    {
        private readonly IDisposable _disposable;

        public CrossSceneAudioStateTransition(FocusedScene scene, ReadOnlyReactiveProperty<FocusedScene> subscriber, IBackgroundAudioContext context, TFactory stateFactory)
        {
            _disposable = subscriber.Subscribe(s =>
            {
                if (s == scene)
                {
                    context.State = stateFactory.Create();
                }
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
