using MessagePipe;
using System;
using Zenject;

namespace Aniki.Audio {
	internal class EventAudio<T> : IDisposable where T : struct {
		private readonly IDisposable	disposable;

		public EventAudio(IOneShotAudioPlayer player,
			ISubscriber<T> subscriber, [InjectOptional] MessageHandlerFilter<T> filter,
			AudioPlayerParameters parameters) {
			if (filter == null)
				disposable = subscriber.Subscribe(_ => player.Play(parameters));
			else
				disposable = subscriber.Subscribe(_ => player.Play(parameters), filter);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
