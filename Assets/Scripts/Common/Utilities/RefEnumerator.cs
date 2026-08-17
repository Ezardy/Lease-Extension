using System.Collections;
using System.Collections.Generic;

namespace Aniki.Common {
	public class RefEnumerator<T> : IEnumerator<T> where T : class {
		private readonly IEnumerator<IRef<T>>	refEnumerator;

		public T	Current => refEnumerator.Current.I;

		object IEnumerator.Current => Current;

		public bool	MoveNext() {
			return refEnumerator.MoveNext();
		}

		public void	Reset() {
			refEnumerator.Reset();
		}

		public void	Dispose() {
			refEnumerator.Dispose();
		}

		public RefEnumerator(IEnumerable<IRef<T>> refEnumerable) {
			refEnumerator = refEnumerable.GetEnumerator();
		}
	}
}
