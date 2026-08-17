using R3;
using System;
using UnityEngine;

namespace Aniki.Audio {
	internal class BackgroundAudioPlayer : MonoBehaviour {
		[SerializeField] private AudioSource							audioSource1;
		[SerializeField] private AudioSource							audioSource2;
		[SerializeField] private AudioClip								clip;
		[SerializeField, Range(0f, 1f)] private float					transition = 1;
		[SerializeField] private SerializableReactiveProperty<float>	volume = new(1);

		private IDisposable	disposable;
		private bool		isFirst = false;

		private void	Start() {
			IDisposable	d1 = Observable.EveryValueChanged(this, c => c.transition)
				.Skip(1)
				.Subscribe(t => SetAudioSources(t, volume.CurrentValue));
			IDisposable	d2 = volume.Subscribe(v => SetAudioSources(transition, v));
			IDisposable	d3 = Observable.EveryValueChanged(this, c => c.clip)
				.Skip(1).Subscribe(SetClip);

			disposable = Disposable.Combine(d1, d2, d3);
		}

		private void	SetClip(AudioClip clip) {
			if (isFirst)
				SetClipAndPlay(audioSource2, clip);
			else
				SetClipAndPlay(audioSource1, clip);
			if (transition == 1)
				SetAudioSources(1, volume.CurrentValue);
			else
				transition = 1;
		}

		private static void	SetClipAndPlay(AudioSource source, AudioClip clip) {
			source.generator = clip;
			if (clip != null)
				source.Play();
		}

		private void	SetAudioSources(float t, float v) {
			if (isFirst)
				SetAudioSource(audioSource1, audioSource2, t, v);
			else
				SetAudioSource(audioSource2, audioSource1, t, v);
		}

		private void	SetAudioSource(AudioSource a1, AudioSource a2, float t, float v) {
			if (t == 0) {
				a1.generator = null;
				isFirst = !isFirst;
			} else
				a1.volume = v * t;
			a2.volume = v * (1 - t);
		}

		private void	OnDestroy() {
			disposable.Dispose();
		}
	}
}
