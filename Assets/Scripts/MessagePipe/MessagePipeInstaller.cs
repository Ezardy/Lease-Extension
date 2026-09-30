using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Input.Contract;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.MessagePipe
{
    [CreateAssetMenu(fileName = "MessagePipeInstaller", menuName = "Installers/Message Pipe Installer")]
    [MovedFrom("Aniki.Common")]
    internal partial class MessagePipeInstaller : ScriptableObjectInstaller<MessagePipeInstaller>
    {
        public override void InstallBindings()
        {
            InstallMessagePipe();
        }

        private void InstallMessagePipe()
        {
            MessagePipeOptions options = Container.BindMessagePipe();
            Container.BindMessageBroker<CharacterState>(options);
            Container.BindMessageBroker<RestartRequested>(options);
            Container.BindMessageBroker<FocusedScene>(options);
            InstallInput(options);
            InstallCollisions(options);
            Container.BindFactory<MessagePipeDiagnostics, MessagePipeDiagnostics.Factory>().FromFactory<MessagePipeDiagnosticsFactory>();
            Container.BindInterfacesTo<MessagePipeDiagnosticsBootstrap>().AsSingle();
        }

        private void InstallCollisions(MessagePipeOptions options)
        {
            Container.BindMessageBroker<ObstacleCollided>(options);
            Container.BindMessageBroker<FloorCollided>(options);
            Container.BindMessageBroker<ToothPicked>(options);
            Container.BindMessageBroker<BarPassed>(options);
        }

        private void InstallInput(MessagePipeOptions options)
        {
            Container.BindMessageBroker<PunchRequested>(options);
            Container.BindMessageBroker<Tapped>(options);
        }
    }
}
