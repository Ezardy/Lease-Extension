using Aniki.State;
using Aniki.World;
using MessagePipe;
using R3;
using System;
using LeaseExtension.Input.Contract;

namespace Aniki.Character {
	internal class WaitState : APoolablePublishingState<WaitState, ICharacterContext, CharacterState> {
		private readonly CollisionCheckStateBase		stateBase;
		private readonly PunchState.Factory				punchFactory;
		private readonly ICharacterModel				characterModel;
		private readonly INoPunchZone					noPunchZone;
		private readonly ISubscriber<PunchRequested>	punchInput;

		private IDisposable	disposable;

		public WaitState(ICharacterContext context,
			StatePublisher<CharacterState>.Factory publisherFactory,
			CollisionCheckStateBase stateBase,
			PunchState.Factory punchFactory,
			ISubscriber<PunchRequested> punchInput,
			INoPunchZone noPunchZone,
			ICharacterModel characterModel)
			: base(context, publisherFactory.Create(CharacterState.WAIT)) {
			this.stateBase = stateBase;
			this.punchInput = punchInput;
			this.punchFactory = punchFactory;
			this.characterModel = characterModel;
			this.noPunchZone = noPunchZone;
		}

		private void	Punch(PunchRequested _) {
			if (!noPunchZone.InZone)
				context.State = punchFactory.Create(characterModel.PunchHeight);
		}

		public override void	Dispose() {
			disposable.Dispose();
			disposable = null;
			stateBase.Dispose();
			base.Dispose();
		}

		public override void	Start() {
			base.Start();
			stateBase.Start();
			disposable = punchInput.Subscribe(Punch);
		}
	}
}
