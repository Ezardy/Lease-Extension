using LeaseExtension.Common.Layer;
using LeaseExtension.Common.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LeaseExtension.Common.Installer
{
    [CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common Installer")]
    internal class CommonInstaller : ScriptableObjectInstaller<CommonInstaller>
    {
        [SerializeField] private LayerNames _layerNames;
        [SerializeField] private Camera _cameraPrefab;
        [SerializeField] private EventSystem _eventSystemPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_layerNames);
            Container.BindInterfacesTo<ScreenSizeObserver>().AsSingle();
            Container.Bind<Camera>().FromComponentInNewPrefab(_cameraPrefab.gameObject).AsSingle().NonLazy();
            Container.Bind<EventSystem>().FromComponentInNewPrefab(_eventSystemPrefab.gameObject).AsSingle().NonLazy();
        }
    }
}