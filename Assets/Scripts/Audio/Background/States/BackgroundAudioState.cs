using JetBrains.Annotations;
using LeaseExtension.State;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio.Background.States
{
    internal abstract class BackgroundAudioState<T> : State<IBackgroundAudioContext>, IPoolable<IMemoryPool>
        where T : BackgroundAudioState<T>
    {
        private readonly int _triggerHash;
        private IMemoryPool _pool;

        protected BackgroundAudioState(IBackgroundAudioContext context, string animatorStateName) : base(context)
        {
            _triggerHash = Animator.StringToHash(animatorStateName);
        }

        public override void Start()
        {
            Context.Animator.SetTrigger(_triggerHash);
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
        {}
    }
}