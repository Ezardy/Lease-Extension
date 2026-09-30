using Zenject;

namespace LeaseExtension.MessagePipe
{
    internal partial class MessagePipeInstaller
    {
        private class MessagePipeDiagnosticsBootstrap : IInitializable
        {
            private readonly MessagePipeDiagnostics.Factory _factory;

            public MessagePipeDiagnosticsBootstrap(MessagePipeDiagnostics.Factory factory)
            {
                _factory = factory;
            }

            public void Initialize()
            {
                _factory.Create();
            }
        }
    }
}
