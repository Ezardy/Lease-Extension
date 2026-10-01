using System;
using LeaseExtension.Audio.Contract;
using LeaseExtension.Common.Utilities;
using UnityEngine.Audio;

namespace LeaseExtension.Audio
{
    [Serializable]
    internal class VolumeGeneratorPair
    {
        public float Volume = 1;
        public IRef<IAudioGenerator> Generator;

        public AudioPlayerParameters ToParameters()
        {
            return new(Generator.I, Volume);
        }
    }
}
