using JetBrains.Annotations;
using LeaseExtension.SceneManagement.States;
using Zenject;
using Context = LeaseExtension.State.Context;

namespace LeaseExtension.SceneManagement
{
    [UsedImplicitly]
    internal class SceneManagementContext : Context, ISceneContext, IInitializable
    {
        private readonly WelcomeSceneState.Factory _welcomeFactory;

        public SceneManagementContext(WelcomeSceneState.Factory welcomeFactory)
        {
            _welcomeFactory = welcomeFactory;
        }

        public void Initialize()
        {
            State = _welcomeFactory.Create();
        }
    }
}