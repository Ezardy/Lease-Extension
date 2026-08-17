using Aniki.State;
using UnityEngine;
using Zenject;

namespace Aniki.Audio {
	internal abstract class ABackgroundAudioState<T> : AState<IBackgroundAudioContext>, IPoolable<IMemoryPool> where T : ABackgroundAudioState<T> {
		protected readonly int	triggerHash;

		private IMemoryPool	pool;

		protected ABackgroundAudioState(IBackgroundAudioContext context, string animatorStateName) : base(context) {
			triggerHash = Animator.StringToHash(animatorStateName);
		}

		public void OnDespawned() {
			pool = null;
		}

		public void OnSpawned(IMemoryPool pool) {
			this.pool = pool;
		}

		public override void	Start() {
			context.Animator.SetTrigger(triggerHash);
		}

		public override void	Dispose() {
			pool.Despawn(this);
		}

		public class Factory : PlaceholderFactory<T> { }
	}
}
