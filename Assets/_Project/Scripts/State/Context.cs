using LeaseExtension.State.Contract;

namespace LeaseExtension.State
{
    public abstract class Context : IContext
    {
        private IState _state;
        private bool _disposed;

        public IState State
        {
            get => _state;
            set
            {
                if (_disposed)
                {
                    value.Dispose();
                }
                else
                {
                    _state?.Dispose();
                    _state = value;
                    _state.Start();
                }
            }
        }

        public virtual void Dispose()
        {
            _state?.Dispose();
            _state = null;
            _disposed = true;
        }
    }
}