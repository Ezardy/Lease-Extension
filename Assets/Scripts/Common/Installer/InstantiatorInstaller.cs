using LeaseExtension.Common.Utilities;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Installer
{
    [CreateAssetMenu(fileName = "InstantiatorInstaller", menuName = "Installers/Instantiator")]
    internal class InstantiatorInstaller : ScriptableObjectInstaller<InstantiatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, GameObject, PrefabFactory.Factory>().FromFactory<PrefabFactory>();
        }
    }
}