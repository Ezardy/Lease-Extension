using LeaseExtension.State.Contract;

namespace LeaseExtension.State
{
    public abstract class AState<T> : IState where T : IContext
    {
        protected readonly T Context;

        public AState(T context)
        {
            this.Context = context;
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
