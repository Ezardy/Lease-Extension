using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.Audio
{
    [MovedFrom("Aniki.Audio")]
    internal class AudioSourcePool : IObjectPool<AudioSource>, IInitializable, IDisposable
    {
        private readonly AudioSourceFactory _factory;
        private readonly IObjectPool<AudioSource> _pool;
        private Transform _poolTransform;
        private bool _isQuiting = false;

        public int CountInactive => _pool.CountInactive;

        public AudioSourcePool(AudioSourceFactory factory)
        {
            _factory = factory;
            _pool = new ObjectPool<AudioSource>(Create, OnGet, OnRelease, OnDestroy, true);
        }

        public void Initialize()
        {
            _poolTransform = new GameObject("Audio Source pool").transform;
            UnityEngine.Object.DontDestroyOnLoad(_poolTransform);
        }

        public void Clear()
        {
            _pool.Clear();
        }

        public AudioSource Get()
        {
            return _pool.Get();
        }

        public PooledObject<AudioSource> Get(out AudioSource v)
        {
            return _pool.Get(out v);
        }

        public void Release(AudioSource element)
        {
            _pool.Release(element);
        }

        public void Dispose()
        {
            _isQuiting = true;
        }

        private void OnDestroy(AudioSource instance)
        {
            UnityEngine.Object.Destroy(instance.gameObject);
        }

        private AudioSource Create()
        {
            AudioSource instance = _factory.Create();
            OnRelease(instance);
            return instance;
        }

        private void OnGet(AudioSource instance)
        {
            instance.gameObject.SetActive(true);
            instance.transform.parent = null;
        }

        private void OnRelease(AudioSource instance)
        {
            instance.generator = null;
            if (!_isQuiting)
                instance.transform.parent = _poolTransform;
            instance.gameObject.SetActive(false);
        }
    }
}
