using Aniki.State;
using Aniki.UI;
using MessagePipe;
using System;

namespace Aniki.Character {
	internal class OverState : APoolablePublishingState<OverState, ICharacterContext, CharacterState> {
		private readonly ISubscriber<AcceptSentenceMessage>	resetSubscriber;
		private readonly IdleState.Factory					idleFactory;

		private IDisposable	disposable;

		public OverState(ICharacterContext context,
			ISubscriber<AcceptSentenceMessage> resetSubscriber,
			StatePublisher<CharacterState>.Factory publisherFactory,
			IdleState.Factory idleFactory)
			: base(context, publisherFactory.Create(CharacterState.OVER)) {
			this.idleFactory = idleFactory;
			this.resetSubscriber = resetSubscriber;
		}

		public override void Start() {
			base.Start();
			disposable = resetSubscriber.Subscribe(_ =>
				context.State = idleFactory.Create()
			);
		}

		public override void	Dispose() {
			disposable.Dispose();
			base.Dispose();
		}
	}
}
