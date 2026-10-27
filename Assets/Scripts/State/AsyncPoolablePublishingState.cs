using System;
using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using LeaseExtension.Common.Utilities;
using LeaseExtension.State.Contract;
using R3;
using Zenject;

namespace LeaseExtension.State
{
    public abstract class AsyncPoolablePublishingState<T, TContext, TState> :
        PoolablePublishingStateBase<TContext, TState>,
        IPoolable<IMemoryPool> where T : AsyncPoolablePublishingState<T, TContext, TState>, IAsyncLoadable
        where TContext : IContext
        where TState : Enum
    {
        protected AsyncPoolablePublishingState(TContext context, ReactiveProperty<TState> statePublisher, TState state)
            :
            base(context, statePublisher, state)
        {}

        public void OnSpawned(IMemoryPool pool)
        {
            SetPool(pool);
            StartLoad();
        }

        protected abstract void StartLoad();

        [UsedImplicitly]
        public class Factory : AsyncPlaceholderFactory<T>
        {}
    }
}