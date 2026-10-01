using System;
using JetBrains.Annotations;
using LeaseExtension.Audio.Contract;
using MessagePipe;
using Zenject;

namespace LeaseExtension.Audio
{
    [UsedImplicitly]
    internal class EventAudio<T> : IDisposable where T : struct
    {
        private readonly IDisposable _disposable;

        public EventAudio(
            IOneShotAudioPlayer player,
            ISubscriber<T> subscriber,
            [InjectOptional] MessageHandlerFilter<T> filter,
            AudioPlayerParameters parameters)
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
