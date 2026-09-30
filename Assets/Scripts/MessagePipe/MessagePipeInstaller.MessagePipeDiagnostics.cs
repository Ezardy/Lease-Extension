using Zenject;

namespace LeaseExtension.MessagePipe
{
    internal partial class MessagePipeInstaller
    {
        private class MessagePipeDiagnostics
        {
            public class Factory : PlaceholderFactory<MessagePipeDiagnostics>
            {
            }
        }
    }
}
