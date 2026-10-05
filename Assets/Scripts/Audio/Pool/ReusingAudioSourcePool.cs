using System.Collections.Specialized;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.Audio.Pool
{
    [UsedImplicitly]
    internal class ReusingAudioSourcePool : IObjectPool<AudioSource>
    {
        private readonly IObjectPool<AudioSource> _pool;
        private readonly OrderedDictionary _inUse;
        private readonly int _max;

        public int CountInactive => _max - _inUse.Count;

        public ReusingAudioSourcePool(IObjectPool<AudioSource> pool, int max)
        {
            _pool = pool;
            _max = max;
            _inUse = new(max);
        }

        public void Clear()
        {
            foreach (AudioSource i in _inUse)
                _pool.Release(i);
            _inUse.Clear();
        }

        public AudioSource Get()
        {
            AudioSource instance;
            ReleaseIfMax();
            instance = _pool.Get();
            _inUse.Add(instance, instance);
            return instance;
        }

        public PooledObject<AudioSource> Get(out AudioSource v)
        {
            PooledObject<AudioSource> instance;
            ReleaseIfMax();
            instance = _pool.Get(out v);
            _inUse.Add(v, v);
            return instance;
        }

        public void Release(AudioSource element)
        {
            if (_inUse.Contains(element))
            {
                _inUse.Remove(element);
                _pool.Release(element);
            }
        }

        private void ReleaseIfMax()
        {
            if (_max == _inUse.Count)
            {
                _pool.Release(_inUse[0] as AudioSource);
                _inUse.RemoveAt(0);
            }
        }
    }
}
