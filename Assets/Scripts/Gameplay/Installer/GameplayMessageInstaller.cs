using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.MessagePipe;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.Gameplay.Installer
{
    [CreateAssetMenu(fileName = "GameplayMessageInstaller", menuName = "Installers/Gameplay Messages")]
    internal class GameplayMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<CharacterState>(Options);
            Container.BindMessageBroker<RestartRequested>(Options);
        }
    }
}