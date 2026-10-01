using LeaseExtension.SceneManagment.States;
using LeaseExtension.State;
using Zenject;

namespace LeaseExtension.SceneManagment
{
    internal class SceneManagmentContext : AContext, ISceneContext, IInitializable
    {
        private readonly WelcomeSceneState.Factory _welcomeFactory;

        public SceneManagmentContext(WelcomeSceneState.Factory welcomeFactory)
        {
            _welcomeFactory = welcomeFactory;
        }

        public void Initialize()
        {
            State = _welcomeFactory.Create();
        }
    }
}
