using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.World
{
    [CreateAssetMenu(fileName = "WorldInstaller", menuName = "Installers/World Installer")]
    [MovedFrom("Aniki.World")]
    internal class WorldInstaller : ScriptableObjectInstaller<WorldInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("cameraPrefab")]
        private Camera _cameraPrefab;

        [SerializeField]
        [FormerlySerializedAs("eventSystemPrefab")]
        private EventSystem _eventSystemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromComponentInNewPrefab(_cameraPrefab.gameObject).AsSingle().NonLazy();
            Container.Bind<EventSystem>().FromComponentInNewPrefab(_eventSystemPrefab.gameObject).AsSingle().NonLazy();
        }
    }
}
