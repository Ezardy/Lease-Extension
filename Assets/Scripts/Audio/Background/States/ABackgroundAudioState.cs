using JetBrains.Annotations;
using LeaseExtension.State;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio.Background.States
{
    internal abstract class ABackgroundAudioState<T> : AState<IBackgroundAudioContext>, IPoolable<IMemoryPool> where T : ABackgroundAudioState<T>
    {
        protected readonly int TriggerHash;
        private IMemoryPool _pool;

        protected ABackgroundAudioState(IBackgroundAudioContext context, string animatorStateName) : base(context)
        {
            TriggerHash = Animator.StringToHash(animatorStateName);
        }

        public override void Start()
        {
            Context.Animator.SetTrigger(TriggerHash);
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public override void Dispose()
        {
            _pool.Despawn(this);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}
