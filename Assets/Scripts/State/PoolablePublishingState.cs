using System;
using JetBrains.Annotations;
using LeaseExtension.State.Contract;
using R3;
using Zenject;

namespace LeaseExtension.State
{
    public abstract class PoolablePublishingState<TContext, TState> : State<TContext>
        where TContext : IContext where TState : Enum
    {
        private readonly ReactiveProperty<TState> _statePublisher;
        private readonly TState _state;
        private IMemoryPool _pool;

        protected PoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
            base(context)
        {
            _state = state;
            _statePublisher = statePublisher;
        }

        public override void Start()
        {
            _statePublisher.Value = _state;
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

    public abstract class PoolablePublishingState<T, TContext, TState> : PoolablePublishingState<TContext, TState>,
        IPoolable<IMemoryPool> where T : PoolablePublishingState<T, TContext, TState>
        where TContext : IContext
        where TState : Enum
    {
        protected PoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
            base(context, statePublisher, state)
        {}

        public void OnSpawned(IMemoryPool pool)
        {
            SetPool(pool);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<T>
        {}
    }

    public abstract class PoolablePublishingState<TP, T, TContext, TState> :
        PoolablePublishingState<TContext, TState>,
        IPoolable<TP, IMemoryPool> where T : PoolablePublishingState<TP, T, TContext, TState>
        where TContext : IContext
        where TState : Enum
    {
        protected PoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
            base(context, statePublisher, state)
        {}

        public virtual void OnSpawned(TP param, IMemoryPool pool)
        {
            SetPool(pool);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<TP, T>
        {}
    }
}