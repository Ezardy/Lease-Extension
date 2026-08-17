using Aniki.State;
using Aniki.World;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.Character {
	internal class FallState : APoolablePublishingState<FallState, ICharacterContext, CharacterState> {
		private readonly ISubscriber<FloorCollisionMessage>	subscriber;
		private readonly OverState.Factory					overStateFactory;
		private readonly Rigidbody2D						rigidbody;

		private IDisposable	subscription;

		public FallState(ICharacterContext context,
			StatePublisher<CharacterState>.Factory publisherFactory,
			Rigidbody2D rigidbody,
			ISubscriber<FloorCollisionMessage> subscriber,
			OverState.Factory overStateFactory)
			: base(context, publisherFactory.Create(CharacterState.FALL)) {
			this.rigidbody = rigidbody;
			this.subscriber = subscriber;
			this.overStateFactory = overStateFactory;
		}

		public override void	Start() {
			base.Start();
			subscription = subscriber.Subscribe(_ => context.State = overStateFactory.Create());

			rigidbody.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
		}

		public override void	Dispose() {
			subscription.Dispose();
			subscription = null;
			base.Dispose();
		}
	}
}
