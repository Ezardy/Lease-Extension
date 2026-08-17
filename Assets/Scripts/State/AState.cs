namespace Aniki.State {
	public abstract class AState<T> : IState where T : IContext {
		protected readonly T	context;

		public AState(T context) {
			this.context = context;
		}

		public virtual void	Dispose() { }
		public virtual void	Start() { }
		public virtual void	Update() { }
	}
}
