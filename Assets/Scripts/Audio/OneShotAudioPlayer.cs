using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LeaseExtension.Audio.Contract;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Audio
{
    [MovedFrom("Aniki.Audio")]
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
            parameters.LoadParameters(audioSource);
            audioSource.Play();
            await UniTask.WaitWhile(() => audioSource.isPlaying, cancellationToken: _tokenSource.Token);
            AudioPlayerParameters.Default.LoadParameters(audioSource);
            _pool.Release(audioSource);
        }
    }
}
