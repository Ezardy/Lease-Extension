using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Specialized;

namespace Aniki.Audio {
	internal class ReusingAudioSourcePool : IObjectPool<AudioSource> {
		private readonly IObjectPool<AudioSource>	pool;
		private readonly OrderedDictionary			inUse;
		private readonly int						max;

		public ReusingAudioSourcePool(IObjectPool<AudioSource> pool, int max) {
			this.pool = pool;
			this.max = max;
			inUse = new(max);
		}

		public int	CountInactive => max - inUse.Count;

		public void	Clear() {
			foreach (AudioSource i in inUse)
				pool.Release(i);
			inUse.Clear();
		}

		public AudioSource	Get() {
			AudioSource	instance;

			ReleaseIfMax();
			instance = pool.Get();
			inUse.Add(instance, instance);
			return instance;
		}

		public PooledObject<AudioSource>	Get(out AudioSource v) {
			PooledObject<AudioSource>	instance;

			ReleaseIfMax();
			instance = pool.Get(out v);
			inUse.Add(v, v);
			return instance;
		}

		public void	Release(AudioSource element) {
			if (inUse.Contains(element)) {
				inUse.Remove(element);
				pool.Release(element);
			}
		}

		private void	ReleaseIfMax() {
			if (max == inUse.Count) {
				pool.Release(inUse[0] as AudioSource);
				inUse.RemoveAt(0);
			}
		}
	}
}
