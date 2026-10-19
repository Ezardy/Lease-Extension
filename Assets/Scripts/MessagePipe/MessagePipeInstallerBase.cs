using MessagePipe;
using Zenject;

namespace LeaseExtension.MessagePipe
{
    public abstract class MessagePipeInstallerBase : ScriptableObjectInstaller
    {
        protected MessagePipeOptions Options { get; private set; }

        [Inject]
        private void Construct(MessagePipeOptions options)
        {
            Options = options;
        }
    }
}