using Aniki.State;
using Aniki.World;
using MessagePipe;
using System;

namespace Aniki.Character {
	internal class CollisionCheckStateBase : AState<IContext> {
		private readonly ISubscriber<ObstacleCollisionMessage>	wallSubscriber;
		private readonly ISubscriber<FloorCollisionMessage>		floorSubscriber;
		private readonly FallState.Factory						fallFactory;
		private readonly OverState.Factory						overFactory;

		private IDisposable	wallSubscription;
		private IDisposable	floorSubscription;

		public CollisionCheckStateBase(IContext context,
			ISubscriber<ObstacleCollisionMessage> wallSubscriber,
			ISubscriber<FloorCollisionMessage> floorSubscriber,
			FallState.Factory fallFactory, OverState.Factory overFactory) : base(context) {
			this.wallSubscriber = wallSubscriber;
			this.floorSubscriber = floorSubscriber;
			this.fallFactory = fallFactory;
			this.overFactory = overFactory;
		}

		public override void	Start() {
			wallSubscription = wallSubscriber.Subscribe((_) => context.State =  fallFactory.Create());
			floorSubscription = floorSubscriber.Subscribe((_) => context.State =  overFactory.Create());
		}

		public override void	Dispose() {
			wallSubscription.Dispose();
			floorSubscription.Dispose();
			wallSubscription = null;
			floorSubscription = null;
		}
	}
}
