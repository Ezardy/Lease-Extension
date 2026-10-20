using LeaseExtension.Input.Contract;
using LeaseExtension.MessagePipe;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "UIInputMessageInstaller", menuName = "Installers/Input/UI Messages")]
    internal class UIInputMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<Submitted>(Options);
        }
    }
}