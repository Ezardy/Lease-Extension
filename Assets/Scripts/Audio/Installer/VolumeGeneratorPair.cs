using System;
using LeaseExtension.Audio.Contract;
using LeaseExtension.Common.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace LeaseExtension.Audio.Installer
{
    [Serializable]
    internal class VolumeGeneratorPair
    {
        [SerializeField, Range(0, 1)]
        private float _volume = 1;
        [SerializeField]
        private Ref<IAudioGenerator> _generator;

        public AudioPlayerParameters ToParameters()
        {
            return new(_generator.I, _volume);
        }
    }
}
