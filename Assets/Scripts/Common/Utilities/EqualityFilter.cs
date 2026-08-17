using MessagePipe;
using System;

namespace Aniki.Common {
	public class EqualityFilter<T> : MessageHandlerFilter<T> where T : struct {
		private readonly T	sample;

		public EqualityFilter(T sample) {
			this.sample = sample;
		}

		public override void	Handle(T message, Action<T> next) {
			if (sample.Equals(message))
				next(message);
		}
	}
}
