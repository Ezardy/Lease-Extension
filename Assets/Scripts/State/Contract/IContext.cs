using System;

namespace Aniki.State {
	public interface IContext: IDisposable {
		public IState	State { get; set; }
	}
}
