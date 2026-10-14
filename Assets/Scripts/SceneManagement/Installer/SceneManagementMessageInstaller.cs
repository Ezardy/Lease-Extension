using LeaseExtension.MessagePipe;
using LeaseExtension.SceneManagement.Contract.Message;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.SceneManagement.Installer
{
    [CreateAssetMenu(fileName = "SceneManagementMessageInstaller", menuName = "Installers/Scene Management Messages")]
    internal class SceneManagementMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<FocusedScene>(Options);
        }
    }
}