using JetBrains.Annotations;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace LeaseExtension.MessagePipe
{
    [CreateAssetMenu(fileName = "MessagePipeInstaller", menuName = "Installers/Message Pipe Installer")]
    internal class MessagePipeInstaller : ScriptableObjectInstaller<MessagePipeInstaller>
    {
        [SerializeField] private MessagePipeInstallerBase[] _messageInstallers;

        public override void InstallBindings()
        {
            MessagePipeOptions options = Container.BindMessagePipe();
            foreach (MessagePipeInstallerBase installer in _messageInstallers)
            {
                Container.Inject(installer, new object[] { options });
                installer.InstallBindings();
            }

            Container.BindInterfacesTo<MessagePipeDiagnosticsBootstrap>().AsSingle();
        }

        [UsedImplicitly]
        private class MessagePipeDiagnosticsBootstrap : IInitializable
        {
            private readonly DiContainer _container;

            public MessagePipeDiagnosticsBootstrap(DiContainer container)
            {
                _container = container;
            }

            public void Initialize()
            {
                GlobalMessagePipe.SetProvider(_container.AsServiceProvider());
            }
        }
    }
}