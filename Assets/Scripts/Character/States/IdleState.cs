using Aniki.Input;
using Aniki.State;
using MessagePipe;
using System;

namespace Aniki.Character {
	internal class IdleState : APoolablePublishingState<IdleState, ICharacterContext, CharacterState> {
		private readonly ICharacterModel				characterModel;
		private readonly PunchState.Factory				punchFactory;
		private readonly ISubscriber<PunchInputMessage>	punchInput;

		private IDisposable	disposable;

		public IdleState(ICharacterContext context,
			PunchState.Factory punchFactory,
			StatePublisher<CharacterState>.Factory publisherFactory,
			ISubscriber<PunchInputMessage> punchInput, ICharacterModel characterModel)
			: base(context, publisherFactory.Create(CharacterState.IDLE)) {
			this.characterModel = characterModel;
			this.punchFactory = punchFactory;
			this.punchInput = punchInput;
		}

		private void	Punch(PunchInputMessage _) {
			context.State = punchFactory.Create(characterModel.InitialPunchHeight);
		}

		public override void	Dispose() {
			disposable.Dispose();
			disposable = null;
			base.Dispose();
		}

		public override void	Start() {
			base.Start();
			disposable = punchInput.Subscribe(Punch);
		}
	}
}
