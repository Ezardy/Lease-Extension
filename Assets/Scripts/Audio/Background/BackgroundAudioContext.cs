using LeaseExtension.Audio.Background.States;
using LeaseExtension.State;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.Audio.Background
{
    [MovedFrom("Aniki.Audio")]
    internal class BackgroundAudioContext : AContext, IBackgroundAudioContext, IInitializable
    {
        private readonly WelcomeAudioState.Factory _welcomeFactory;

        public Animator Animator { get; }

        public BackgroundAudioContext(Animator animator, WelcomeAudioState.Factory welcomeFactory)
        {
            this.Animator = animator;
            _welcomeFactory = welcomeFactory;
        }

        public void Initialize()
        {
            State = _welcomeFactory.Create();
        }
    }
}
