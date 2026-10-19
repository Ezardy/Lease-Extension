using LeaseExtension.Input.Contract;
using LeaseExtension.MessagePipe;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.Input.Installer
{
    [CreateAssetMenu(fileName = "InputMessageInstaller", menuName = "Installers/Input Messages")]
    internal class InputMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<PunchRequested>(Options);
            Container.BindMessageBroker<Tapped>(Options);
        }
    }
}