using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LeaseExtension.World
{
    [CreateAssetMenu(fileName = "WorldInstaller", menuName = "Installers/World Installer")]
    internal class WorldInstaller : ScriptableObjectInstaller<WorldInstaller>
    {
        [SerializeField]
        private Camera _cameraPrefab;

        [SerializeField]
        private EventSystem _eventSystemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromComponentInNewPrefab(_cameraPrefab.gameObject).AsSingle().NonLazy();
            Container.Bind<EventSystem>().FromComponentInNewPrefab(_eventSystemPrefab.gameObject).AsSingle().NonLazy();
        }
    }
}
