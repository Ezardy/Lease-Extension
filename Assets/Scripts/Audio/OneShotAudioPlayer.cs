using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Audio.Contract;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.Audio
{
    [UsedImplicitly]
    internal class OneShotAudioPlayer : IOneShotAudioPlayer, IDisposable
    {
        private readonly IObjectPool<AudioSource> _pool;
        private readonly CancellationTokenSource _tokenSource = new();

        public OneShotAudioPlayer(IObjectPool<AudioSource> pool)
        {
            _pool = pool;
        }

        public void Play(AudioPlayerParameters parameters)
        {
            PlayAsync(parameters).Forget();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
        }

        private async UniTaskVoid PlayAsync(AudioPlayerParameters parameters)
        {
            AudioSource audioSource = _pool.Get();
            parameters.ApplyTo(audioSource);
            audioSource.Play();
            await UniTask.WaitWhile(() => audioSource.isPlaying, cancellationToken: _tokenSource.Token);
            AudioPlayerParameters.Default.ApplyTo(audioSource);
            _pool.Release(audioSource);
        }
    }
}
