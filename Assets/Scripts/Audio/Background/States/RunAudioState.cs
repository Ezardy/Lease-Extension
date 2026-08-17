using Aniki.Character;
using MessagePipe;
using System;

namespace Aniki.Audio {
	internal class RunAudioState : ABackgroundAudioState<RunAudioState> {
		private readonly ISubscriber<CharacterState>	characterStateSubscriber;
		private readonly ResultAudioState.Factory		resultFactory;

		private IDisposable	disposable;

		public RunAudioState(IBackgroundAudioContext context,
			ISubscriber<CharacterState> characterStateSubscriber,
			ResultAudioState.Factory resultFactory) : base(context, "Run") {
			this.characterStateSubscriber = characterStateSubscriber;
			this.resultFactory = resultFactory;
		}

		public override void	Start() {
			base.Start();
			disposable = characterStateSubscriber.Subscribe(_ =>
				context.State = resultFactory.Create(),
				CharacterStateFilter.Over);
		}

		public override void	Dispose() {
			disposable.Dispose();
			base.Dispose();
		}
	}
}
