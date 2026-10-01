using JetBrains.Annotations;
using Zenject;

namespace LeaseExtension.MessagePipe
{
    internal partial class MessagePipeInstaller
    {
        private class MessagePipeDiagnostics
        {
            [UsedImplicitly]
            public class Factory : PlaceholderFactory<MessagePipeDiagnostics>
            {
            }
        }
    }
}
