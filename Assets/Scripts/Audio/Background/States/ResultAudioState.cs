using Aniki.Character;
using MessagePipe;
using System;

namespace Aniki.Audio {
	internal class ResultAudioState : ABackgroundAudioState<ResultAudioState> {
		private readonly ISubscriber<CharacterState>	characterStateSubscriber;
		private readonly IdleAudioState.Factory			idleFactory;

		private IDisposable	disposable;

		public ResultAudioState(IBackgroundAudioContext context,
			ISubscriber<CharacterState> characterStateSubscriber,
			IdleAudioState.Factory idleFactory) : base(context, "Result") {
			this.characterStateSubscriber = characterStateSubscriber;
			this.idleFactory = idleFactory;
		}

		public override void	Start() {
			base.Start();
			disposable = characterStateSubscriber.Subscribe(_ =>
				context.State = idleFactory.Create(),
				CharacterStateFilter.Idle);
		}

		public override void	Dispose() {
			disposable.Dispose();
			base.Dispose();
		}
	}
}
