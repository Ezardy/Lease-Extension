using LeaseExtension.State.Contract;

namespace LeaseExtension.State
{
    public abstract class State<T> : IState where T : IContext
    {
        protected readonly T Context;

        public State(T context)
        {
            Context = context;
        }

        public virtual void Start()
        {}

        public virtual void Update()
        {}

        public virtual void Dispose()
        {}
    }
}