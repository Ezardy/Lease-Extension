using LeaseExtension.State.Contract;
using UnityEngine;

namespace LeaseExtension.Audio.Background
{
    internal interface IBackgroundAudioContext : IContext
    {
        public Animator Animator { get; }
    }
}
