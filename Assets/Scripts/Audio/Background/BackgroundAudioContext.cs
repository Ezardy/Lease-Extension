using JetBrains.Annotations;
using LeaseExtension.Audio.Background.States;
using LeaseExtension.State;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio.Background
{
    [UsedImplicitly]
    internal class BackgroundAudioContext : AContext, IBackgroundAudioContext, IInitializable
    {
        private readonly WelcomeAudioState.Factory _welcomeFactory;

        public Animator Animator { get; }

        public BackgroundAudioContext(Animator animator, WelcomeAudioState.Factory welcomeFactory)
        {
            Animator = animator;
            _welcomeFactory = welcomeFactory;
        }

        public void Initialize()
        {
            State = _welcomeFactory.Create();
        }
    }
}
