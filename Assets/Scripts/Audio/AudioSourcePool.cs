using System;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Aniki.Audio {
	internal class AudioSourcePool : IObjectPool<AudioSource>, IInitializable, IDisposable {
		private readonly AudioSourceFactory			factory;
		private readonly IObjectPool<AudioSource>	pool;

		private Transform	poolTransform;
		private bool		isQuiting = false;

		public AudioSourcePool(AudioSourceFactory factory) {
			this.factory = factory;
			pool = new ObjectPool<AudioSource>(Create, OnGet, OnRelease, OnDestroy, true);
		}

		public void	Initialize() {
			poolTransform = new GameObject("Audio Source pool").transform;
			UnityEngine.Object.DontDestroyOnLoad(poolTransform);
		}

		public int	CountInactive => pool.CountInactive;

		public void	Clear() {
			pool.Clear();
		}

		public AudioSource	Get() {
			return pool.Get();
		}

		public PooledObject<AudioSource>	Get(out AudioSource v) {
			return pool.Get(out v);
		}

		public void	Release(AudioSource element) {
			pool.Release(element);
		}

		private AudioSource	Create() {
			AudioSource	instance = factory.Create();

			OnRelease(instance);
			return instance;
		}

		private void	OnGet(AudioSource instance) {
			instance.gameObject.SetActive(true);
			instance.transform.parent = null;
		}

		private void	OnRelease(AudioSource instance) {
			instance.generator = null;
			if (!isQuiting)
				instance.transform.parent = poolTransform;
			instance.gameObject.SetActive(false);
		}

		private void	OnDestroy(AudioSource instance) {
			UnityEngine.Object.Destroy(instance.gameObject);
		}

		public void	Dispose() {
			isQuiting = true;
		}
	}
}