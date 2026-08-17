using System;

namespace Aniki.State {
	public interface IState : IDisposable {
		public void	Start();
		public void	Update();
	}
}
