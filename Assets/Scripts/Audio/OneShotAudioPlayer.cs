using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

namespace Aniki.Audio {
	internal class OneShotAudioPlayer : IOneShotAudioPlayer, IDisposable {
		private readonly IObjectPool<AudioSource>	pool;
		private readonly CancellationTokenSource	tokenSource = new();

		public OneShotAudioPlayer(IObjectPool<AudioSource> pool) {
			this.pool = pool;
		}

		public void	Play(AudioPlayerParameters parameters) {
			PlayAsync(parameters).Forget();
		}

		private async UniTaskVoid	PlayAsync(AudioPlayerParameters parameters) {
			AudioSource	audioSource = pool.Get();

			parameters.LoadParameters(audioSource);
			audioSource.Play();
			await UniTask.WaitWhile(() => audioSource.isPlaying, cancellationToken: tokenSource.Token);
			AudioPlayerParameters.Default.LoadParameters(audioSource);
			pool.Release(audioSource);
		}

		public void	Dispose() {
			tokenSource.Cancel();
		}
	}
}
