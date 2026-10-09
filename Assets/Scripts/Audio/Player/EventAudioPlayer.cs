using System;
using JetBrains.Annotations;
using LeaseExtension.Audio.Contract;
using MessagePipe;
using Zenject;

namespace LeaseExtension.Audio.Player
{
    [UsedImplicitly]
    internal class EventAudioPlayer<T> : IDisposable where T : struct
    {
        private readonly IDisposable _disposable;

        public EventAudioPlayer(
            IOneShotAudioPlayer player,
            AudioPlayerParameters parameters,
            ISubscriber<T> subscriber,
            [InjectOptional] MessageHandlerFilter<T> filter)
        {
            if (filter == null)
                _disposable = subscriber.Subscribe(_ => player.Play(parameters));
            else
                _disposable = subscriber.Subscribe(_ => player.Play(parameters), filter);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}