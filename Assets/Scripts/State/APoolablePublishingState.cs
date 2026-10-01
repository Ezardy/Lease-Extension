using System;
using LeaseExtension.State.Contract;
using Zenject;

namespace LeaseExtension.State
{
    public abstract class APoolablePublishingState<C, S> : AState<C> where C : IContext where S : Enum
    {
        private readonly StatePublisher<S> _statePublisher;
        private IMemoryPool _pool;

        protected APoolablePublishingState(C context, StatePublisher<S> statePublisher) : base(context)
        {
            _statePublisher = statePublisher;
        }

        public override void Start()
        {
            _statePublisher.Publish();
        }

        public override void Dispose()
        {
            _pool.Despawn(this);
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        protected void SetPool(IMemoryPool pool)
        {
            _pool = pool;
        }
    }

    public abstract class APoolablePublishingState<T, C, S> : APoolablePublishingState<C, S>, IPoolable<IMemoryPool> where T : APoolablePublishingState<T, C, S> where C : IContext where S : Enum
    {
        public APoolablePublishingState(C context, StatePublisher<S> statePublisher) : base(context, statePublisher)
        {
        }

        public void OnSpawned(IMemoryPool pool)
        {
            SetPool(pool);
        }

        public class Factory : PlaceholderFactory<T>
        {
        }
    }

    public abstract class APoolablePublishingState<P, T, C, S> : APoolablePublishingState<C, S>, IPoolable<P, IMemoryPool> where T : APoolablePublishingState<P, T, C, S> where C : IContext where S : Enum
    {
        public APoolablePublishingState(C context, StatePublisher<S> statePublisher) : base(context, statePublisher)
        {
        }

        public virtual void OnSpawned(P param, IMemoryPool pool)
        {
            SetPool(pool);
        }

        public class Factory : PlaceholderFactory<P, T>
        {
        }
    }
}
