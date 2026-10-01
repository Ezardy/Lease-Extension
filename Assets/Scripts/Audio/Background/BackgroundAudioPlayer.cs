using System;
using R3;
using UnityEngine;

namespace LeaseExtension.Audio.Background
{
    internal class BackgroundAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource1;

        [SerializeField]
        private AudioSource _audioSource2;

        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        [Range(0f, 1f)]
        private float _transition = 1;

        [SerializeField]
        private SerializableReactiveProperty<float> _volume = new(1);
        private IDisposable _disposable;
        private bool _isFirst = false;

        private void Start()
        {
            IDisposable d1 = Observable.EveryValueChanged(this, c => c._transition).Skip(1).Subscribe(
t => SetAudioSources(t, _volume.CurrentValue));
            IDisposable d2 = _volume.Subscribe(v => SetAudioSources(_transition, v));
            IDisposable d3 = Observable.EveryValueChanged(this, c => c._clip).Skip(1).Subscribe(SetClip);
            _disposable = Disposable.Combine(d1, d2, d3);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void SetClip(AudioClip clip)
        {
            if (_isFirst)
                SetClipAndPlay(_audioSource2, clip);
            else
                SetClipAndPlay(_audioSource1, clip);
            if (_transition == 1)
                SetAudioSources(1, _volume.CurrentValue);
            else
                _transition = 1;
        }

        private static void SetClipAndPlay(AudioSource source, AudioClip clip)
        {
            source.generator = clip;
            if (clip != null)
                source.Play();
        }

        private void SetAudioSources(float t, float v)
        {
            if (_isFirst)
                SetAudioSource(_audioSource1, _audioSource2, t, v);
            else
                SetAudioSource(_audioSource2, _audioSource1, t, v);
        }

        private void SetAudioSource(AudioSource a1, AudioSource a2, float t, float v)
        {
            if (t == 0)
            {
                a1.generator = null;
                _isFirst = !_isFirst;
            }
            else
            {
                a1.volume = v * t;
            }

            a2.volume = v * (1 - t);
        }
    }
}
