namespace Aniki.State {
	public abstract class AContext: IContext {
		private IState	state;

		public IState	State {
			get => state;
			set {
				state?.Dispose();
				state = value;
				state.Start();
			}
		}

		public virtual void	Dispose() {
			state?.Dispose();
		}
	}
}
