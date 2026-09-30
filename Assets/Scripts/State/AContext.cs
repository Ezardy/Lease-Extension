using LeaseExtension.State.Contract;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.State
{
    [MovedFrom("Aniki.State")]
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
