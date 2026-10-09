using JetBrains.Annotations;
using LeaseExtension.State;
using UnityEngine;

namespace LeaseExtension.Audio.Background
{
    [UsedImplicitly]
    internal class BackgroundAudioContext : AContext, IBackgroundAudioContext
    {
        public Animator Animator { get; }

        public BackgroundAudioContext(Animator animator)
        {
            Animator = animator;
        }
    }
}
