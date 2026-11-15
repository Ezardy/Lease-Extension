using LeaseExtension.Input.Relay;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "PlayerInputRelayInstaller", menuName = "Installers/Input/Player Relay")]
    internal class PlayerInputRelayInstaller : ScriptableObjectInstaller<UIInputRelayInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LeaseExtensionInput.PlayerActions>()
                .FromResolveGetter<LeaseExtensionInput>(input => input.Player);
            Container.BindInterfacesTo<PlayerInputRelay>().AsSingle();
        }
    }
}