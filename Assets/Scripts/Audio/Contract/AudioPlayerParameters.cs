using UnityEngine;
using UnityEngine.Audio;

namespace LeaseExtension.Audio.Contract
{
    public readonly struct AudioPlayerParameters
    {
        public static readonly AudioPlayerParameters Default = new(null, 1, Vector2.zero);
        public readonly IAudioGenerator Generator;
        public readonly float Volume;
        public readonly Vector2 Position;

        public AudioPlayerParameters(IAudioGenerator generator, float volume, Vector2 position)
        {
            this.Generator = generator;
            this.Volume = volume;
            this.Position = position;
        }

        public AudioPlayerParameters(
            IAudioGenerator generator,
            float volume = 1) : this(generator, volume, Vector2.zero)
        {
        }

        public void LoadParameters(AudioSource source)
        {
            source.generator = Generator;
            source.volume = Volume;
            source.transform.position = Position;
        }
    }
}
