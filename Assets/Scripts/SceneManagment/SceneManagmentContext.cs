using LeaseExtension.SceneManagment.States;
using LeaseExtension.State;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.SceneManagment
{
    [MovedFrom("Aniki.SceneManagment")]
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
