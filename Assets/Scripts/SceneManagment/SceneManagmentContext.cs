using JetBrains.Annotations;
using LeaseExtension.SceneManagment.States;
using Zenject;
using Context = LeaseExtension.State.Context;

namespace LeaseExtension.SceneManagment
{
    [UsedImplicitly]
    internal class SceneManagmentContext : Context, ISceneContext, IInitializable
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