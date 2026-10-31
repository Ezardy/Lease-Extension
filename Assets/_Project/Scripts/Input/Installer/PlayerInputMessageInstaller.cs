using LeaseExtension.Input.Contract;
using LeaseExtension.MessagePipe;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "PlayerInputMessageInstaller", menuName = "Installers/Input/Player Messages")]
    internal class PlayerInputMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<PunchRequested>(Options);
        }
    }
}