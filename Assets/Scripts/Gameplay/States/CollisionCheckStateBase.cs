using Aniki.State;
using MessagePipe;
using System;
using LeaseExtension.World.Entities.Message;

namespace Aniki.Character {
	internal class CollisionCheckStateBase : AState<IContext> {
		private readonly ISubscriber<ObstacleCollided>	wallSubscriber;
		private readonly ISubscriber<FloorCollided>		floorSubscriber;
		private readonly FallState.Factory						fallFactory;
		private readonly OverState.Factory						overFactory;

		private IDisposable	wallSubscription;
		private IDisposable	floorSubscription;

		public CollisionCheckStateBase(IContext context,
			ISubscriber<ObstacleCollided> wallSubscriber,
			ISubscriber<FloorCollided> floorSubscriber,
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
