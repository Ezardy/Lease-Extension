using Aniki.State;
using UnityEngine;
using Zenject;

namespace Aniki.Audio {
	internal class BackgroundAudioContext : AContext, IBackgroundAudioContext, IInitializable {
		private readonly Animator					animator;
		private readonly WelcomeAudioState.Factory	welcomeFactory;

		public Animator	Animator => animator;

		public BackgroundAudioContext(Animator animator,
			WelcomeAudioState.Factory welcomeFactory) {
			this.animator = animator;
			this.welcomeFactory = welcomeFactory;
		}

		public void	Initialize() {
			State = welcomeFactory.Create();
		}
	}
}
