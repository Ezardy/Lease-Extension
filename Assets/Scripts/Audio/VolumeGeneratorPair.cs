using Aniki.Common;
using System;
using UnityEngine.Audio;

namespace Aniki.Audio {
	[Serializable]
	internal class VolumeGeneratorPair {
		public float					volume = 1;
		public IRef<IAudioGenerator>	generator;

		public AudioPlayerParameters	ToParameters() {
			return new(generator.I, volume);
		}
	}
}
