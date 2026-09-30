using System;
using LeaseExtension.Audio.Contract;
using LeaseExtension.Common.Utilities;
using UnityEngine.Audio;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace LeaseExtension.Audio
{
    [Serializable]
    [MovedFrom("Aniki.Audio")]
    internal class VolumeGeneratorPair
    {
        [FormerlySerializedAs("volume")]
        public float Volume = 1;

        [FormerlySerializedAs("generator")]
        public IRef<IAudioGenerator> Generator;

        public AudioPlayerParameters ToParameters()
        {
            return new(Generator.I, Volume);
        }
    }
}
