using System;
using JetBrains.Annotations;
using LeaseExtension.State.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.State
{
    public abstract class APoolablePublishingState<TContext, TState> : AState<TContext>
        where TContext : IContext where TState : Enum
    {
        private readonly ReactiveProperty<TState> _statePublisher;
        private readonly TState _state;
        private IMemoryPool _pool;

        protected APoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
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

    public abstract class APoolablePublishingState<T, TContext, TState> : APoolablePublishingState<TContext, TState>,
        IPoolable<IMemoryPool> where T : APoolablePublishingState<T, TContext, TState>
        where TContext : IContext
        where TState : Enum
    {
        protected APoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
            base(context, statePublisher, state)
        {
        }

        public void OnSpawned(IMemoryPool pool)
        {
            SetPool(pool);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<T>
        {
        }
    }

    public abstract class APoolablePublishingState<TP, T, TContext, TState> :
        APoolablePublishingState<TContext, TState>,
        IPoolable<TP, IMemoryPool> where T : APoolablePublishingState<TP, T, TContext, TState>
        where TContext : IContext
        where TState : Enum
    {
        protected APoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state) :
            base(context, statePublisher, state)
        {
        }

        public virtual void OnSpawned(TP param, IMemoryPool pool)
        {
            SetPool(pool);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<TP, T>
        {
        }
    }
}