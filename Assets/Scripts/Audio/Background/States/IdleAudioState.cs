using Aniki.Character;
using MessagePipe;
using System;

namespace Aniki.Audio {
	internal class IdleAudioState : ABackgroundAudioState<IdleAudioState> {
		private readonly ISubscriber<CharacterState>	characterStateSubscriber;
		private readonly RunAudioState.Factory			runFactory;

		private IDisposable	disposable;

		public IdleAudioState(IBackgroundAudioContext context,
			ISubscriber<CharacterState> characterStateSubscriber,
			RunAudioState.Factory runFactory) : base(context, "Idle") {
			this.characterStateSubscriber = characterStateSubscriber;
			this.runFactory = runFactory;
		}

		public override void	Start() {
			base.Start();
			disposable = characterStateSubscriber.Subscribe(_ => {
					disposable.Dispose();
					disposable = null;
					context.State = runFactory.Create();
				},
				CharacterStateFilter.Punch);
		}

		public override void	Dispose() {
			disposable?.Dispose();
			base.Dispose();
		}
	}
}
