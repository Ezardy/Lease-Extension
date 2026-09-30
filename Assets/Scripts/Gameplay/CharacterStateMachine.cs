using Aniki.SceneManagment;
using Aniki.State;
using MessagePipe;
using R3;
using System;
using Zenject;

namespace Aniki.Character {
	internal class CharacterStateMachine : AContext, ICharacterContext, IInitializable, IFixedTickable {
		private readonly IdleState.Factory	idleFactory;
		private readonly IDisposable		disposable;

		public CharacterStateMachine(ICharacterModel characterModel,
			IdleState.Factory idleFactory,
			ISubscriber<CharacterState> stateSubscriber) {
			this.idleFactory = idleFactory;
			disposable = stateSubscriber.Subscribe(s => characterModel.State = s);
		}

		public void	Initialize() {
			State = idleFactory.Create();
		}

		public void	FixedTick() {
			State.Update();
		}

		public override void	Dispose() {
			base.Dispose();
			disposable.Dispose();
		}
	}
}
