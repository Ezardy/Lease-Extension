using System;
using LeaseExtension.State;
using LeaseExtension.State.Contract;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Gameplay.States
{
    [MovedFrom("Aniki.Character")]
    internal class CollisionCheckStateBase : AState<IContext>
    {
        private readonly ISubscriber<ObstacleCollided> _wallSubscriber;
        private readonly ISubscriber<FloorCollided> _floorSubscriber;
        private readonly FallState.Factory _fallFactory;
        private readonly OverState.Factory _overFactory;
        private IDisposable _wallSubscription;
        private IDisposable _floorSubscription;

        public CollisionCheckStateBase(
            IContext context,
            ISubscriber<ObstacleCollided> wallSubscriber,
            ISubscriber<FloorCollided> floorSubscriber,
            FallState.Factory fallFactory,
            OverState.Factory overFactory) : base(context)
        {
            _wallSubscriber = wallSubscriber;
            _floorSubscriber = floorSubscriber;
            _fallFactory = fallFactory;
            _overFactory = overFactory;
        }

        public override void Start()
        {
            _wallSubscription = _wallSubscriber.Subscribe((_) => Context.State = _fallFactory.Create());
            _floorSubscription = _floorSubscriber.Subscribe((_) => Context.State = _overFactory.Create());
        }

        public override void Dispose()
        {
            _wallSubscription.Dispose();
            _floorSubscription.Dispose();
            _wallSubscription = null;
            _floorSubscription = null;
        }
    }
}
