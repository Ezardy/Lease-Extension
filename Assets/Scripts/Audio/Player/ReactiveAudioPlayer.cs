using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Audio.Contract;
using R3;

namespace LeaseExtension.Audio.Player
{
    [UsedImplicitly]
    internal class ReactiveAudioPlayer<T> : IDisposable
    {
        private readonly IDisposable _disposable;

        public ReactiveAudioPlayer(IOneShotAudioPlayer player, AudioPlayerParameters parameters, ReadOnlyReactiveProperty<T> subscriber, T filter)
        {
            _disposable = subscriber.Subscribe(v =>
            {
                if (EqualityComparer<T>.Default.Equals(v, filter))
                {
                    player.Play(parameters);
                }
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
