using MessagePipe;
using System;
using Zenject;

namespace Aniki.State {
	public class StatePublisher<T> where T : Enum {
		private readonly IPublisher<T>	statePublisher;
		private readonly T				state;

		public StatePublisher(
			IPublisher<T> statePublisher, T state) {
			this.statePublisher = statePublisher;
			this.state = state;
		}

		public void	Publish() {
			statePublisher.Publish(state);
		}

		public class Factory : PlaceholderFactory<T, StatePublisher<T>> { }
	}
}
