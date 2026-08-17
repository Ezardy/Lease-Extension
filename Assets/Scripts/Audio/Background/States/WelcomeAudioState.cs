using Aniki.Character;
using Aniki.SceneManagment;
using Aniki.State;
using MessagePipe;
using System;
using Zenject;

namespace Aniki.Audio {
	internal class WelcomeAudioState : AState<IBackgroundAudioContext> {
		private readonly IDisposable	disposable;

		public WelcomeAudioState(IBackgroundAudioContext context,
			ISubscriber<FocusedScene> sceneSubscriber,
			IdleAudioState.Factory idleFactory) : base(context) {
			disposable = sceneSubscriber.Subscribe(_ =>
				context.State = idleFactory.Create(),
				FocusedSceneFilter.Main);
		}

		public override void	Dispose() {
			disposable.Dispose();
		}

		public class Factory : PlaceholderFactory<WelcomeAudioState> { }
	}
}
