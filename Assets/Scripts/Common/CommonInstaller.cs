using LeaseExtension.Common.Layer;
using LeaseExtension.Common.Utilities;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common
{
    [CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common Installer")]
    internal class CommonInstaller : ScriptableObjectInstaller<CommonInstaller>
    {
        [SerializeField]
        private LayerNames _layerNames;

        public override void InstallBindings()
        {
            Container.BindInstance(_layerNames);
            Container.BindInterfacesTo<ScreenSizeObserver>().AsSingle();
        }
    }
}
