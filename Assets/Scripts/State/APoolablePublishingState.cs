using System;
using Zenject;

namespace Aniki.State {
	public abstract class APoolablePublishingState<C, S>
		: AState<C> where C : IContext where S : Enum {
		private readonly StatePublisher<S>	statePublisher;

		private IMemoryPool	pool;
	
		protected APoolablePublishingState(C context,
			StatePublisher<S> statePublisher) : base(context) {
			this.statePublisher = statePublisher;
		}
	
		protected void	SetPool(IMemoryPool pool) {
			this.pool = pool;
		}
	
		public override void	Dispose() {
			pool.Despawn(this);
		}
	
		public void	OnDespawned() {
			pool = null;
		}
	
		public override void	Start() {
			statePublisher.Publish();
		}
	}

	public abstract class APoolablePublishingState<T, C, S>
		: APoolablePublishingState<C, S>, IPoolable<IMemoryPool>
		where T : APoolablePublishingState<T, C, S>
		where C : IContext
		where S : Enum{
		public APoolablePublishingState(C context,
			StatePublisher<S> statePublisher) : base(context, statePublisher) { }

		public void	OnSpawned(IMemoryPool pool) {
			SetPool(pool);
		}

		public class Factory : PlaceholderFactory<T> { }
	}

	public abstract class APoolablePublishingState<P, T, C, S>
		: APoolablePublishingState<C, S>, IPoolable<P, IMemoryPool>
		where T : APoolablePublishingState<P, T, C, S>
		where C : IContext
		where S : Enum {
		public APoolablePublishingState(C context,
			StatePublisher<S> statePublisher) : base(context, statePublisher) { }

		public virtual void	OnSpawned(P param, IMemoryPool pool) {
			SetPool(pool);
		}

		public class Factory : PlaceholderFactory<P, T> { }
	}
}
