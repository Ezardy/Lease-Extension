using LeaseExtension.MessagePipe;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine;

namespace LeaseExtension.World
{
    [CreateAssetMenu(fileName = "WorldMessageInstaller", menuName = "Installers/World Messages")]
    internal class WorldMessageInstaller : MessagePipeInstallerBase
    {
        public override void InstallBindings()
        {
            Container.BindMessageBroker<ObstacleCollided>(Options);
            Container.BindMessageBroker<FloorCollided>(Options);
            Container.BindMessageBroker<ToothPicked>(Options);
            Container.BindMessageBroker<BarPassed>(Options);
        }
    }
}