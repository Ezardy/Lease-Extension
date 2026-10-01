using LeaseExtension.State.Contract;

namespace LeaseExtension.State
{
    public abstract class AContext : IContext
    {
        private IState _state;

        public IState State
        {
            get => _state;
            set
            {
                _state?.Dispose();
                _state = value;
                _state.Start();
            }
        }

        public virtual void Dispose()
        {
            _state?.Dispose();
        }
    }
}
