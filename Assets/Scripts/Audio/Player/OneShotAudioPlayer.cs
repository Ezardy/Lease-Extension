using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Audio.Contract;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.Audio.Player
{
    [UsedImplicitly]
    internal class OneShotAudioPlayer : IOneShotAudioPlayer, IDisposable
    {
        private readonly IObjectPool<AudioSource> _pool;
        private readonly CancellationTokenSource _tokenSource = new();
        private readonly CancellationToken _token;

        public OneShotAudioPlayer(IObjectPool<AudioSource> pool)
        {
            _pool = pool;
            _token = _tokenSource.Token;
        }

        public void Play(AudioPlayerParameters parameters)
        {
            PlayAsync(parameters).Forget();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        private async UniTaskVoid PlayAsync(AudioPlayerParameters parameters)
        {
            AudioSource audioSource = _pool.Get();
            parameters.ApplyTo(audioSource);
            audioSource.Play();
            await UniTask.WaitWhile(() => audioSource != null && audioSource.isPlaying, cancellationToken: _token).SuppressCancellationThrow();
            if (audioSource != null && !_token.IsCancellationRequested)
            {
                AudioPlayerParameters.Default.ApplyTo(audioSource);
                _pool.Release(audioSource);
            }
        }
    }
}