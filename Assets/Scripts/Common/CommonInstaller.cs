using LeaseExtension.Common.Layer;
using LeaseExtension.Common.Utilities;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Common
{
    [CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common Installer")]
    [MovedFrom("Aniki.Common")]
    internal class CommonInstaller : ScriptableObjectInstaller<CommonInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("layerNames")]
        private LayerNames _layerNames;

        public override void InstallBindings()
        {
            Container.BindInstance(_layerNames);
            Container.BindInterfacesTo<ScreenSizeObserver>().AsSingle();
        }
    }
}
