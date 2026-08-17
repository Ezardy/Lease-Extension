using Aniki.State;
using Zenject;

namespace Aniki.SceneManagment {
	internal class SceneManagmentContext : AContext, ISceneContext, IInitializable {
		private readonly WelcomeSceneState.Factory	welcomeFactory;

		public SceneManagmentContext(WelcomeSceneState.Factory welcomeFactory) {
			this.welcomeFactory = welcomeFactory;
		}

		public void	Initialize() {
			State = welcomeFactory.Create();
		}
	}
}
