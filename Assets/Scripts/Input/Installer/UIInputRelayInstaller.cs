using LeaseExtension.Input.Relay;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "UIInputRelayInstaller", menuName = "Installers/Input/UI Relay")]
    internal class UIInputRelayInstaller : ScriptableObjectInstaller<UIInputRelayInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LeaseExtensionInput.UIActions>().FromResolveGetter<LeaseExtensionInput>(input => input.UI);
            Container.BindInterfacesTo<UIInputRelay>().AsSingle();
        }
    }
}