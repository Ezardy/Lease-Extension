using UnityEngine;
using UnityEngine.Audio;

namespace Aniki.Audio {
	public readonly struct AudioPlayerParameters {
		public static readonly AudioPlayerParameters	Default = new(null, 1, Vector2.zero);

		public readonly IAudioGenerator	generator;
		public readonly float			volume;
		public readonly Vector2			position;

		public AudioPlayerParameters(IAudioGenerator generator, float volume,
			Vector2 position) {
			this.generator = generator;
			this.volume = volume;
			this.position = position;
		}

		public AudioPlayerParameters(IAudioGenerator generator, float volume = 1)
			: this(generator, volume, Vector2.zero) { }

		public void	LoadParameters(AudioSource source) {
			source.generator = generator;
			source.volume = volume;
			source.transform.position = position;
		}
	}
}
