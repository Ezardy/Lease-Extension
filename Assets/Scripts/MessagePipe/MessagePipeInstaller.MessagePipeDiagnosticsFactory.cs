using JetBrains.Annotations;
using MessagePipe;
using Zenject;

namespace LeaseExtension.MessagePipe
{
    internal partial class MessagePipeInstaller
    {
        [UsedImplicitly]
        private class MessagePipeDiagnosticsFactory : IFactory<MessagePipeDiagnostics>
        {
            private readonly DiContainer _container;

            public MessagePipeDiagnosticsFactory(DiContainer container)
            {
                _container = container;
            }

            public MessagePipeDiagnostics Create()
            {
                GlobalMessagePipe.SetProvider(_container.AsServiceProvider());
                return new();
            }
        }
    }
}
