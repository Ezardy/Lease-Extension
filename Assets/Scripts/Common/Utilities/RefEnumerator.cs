using System.Collections;
using System.Collections.Generic;

namespace LeaseExtension.Common.Utilities
{
    public class RefEnumerator<T> : IEnumerator<T> where T : class
    {
        private readonly IEnumerator<Ref<T>> _refEnumerator;

        public T Current => _refEnumerator.Current.I;

        object IEnumerator.Current => Current;

        public RefEnumerator(IEnumerable<Ref<T>> refEnumerable)
        {
            _refEnumerator = refEnumerable.GetEnumerator();
        }

        public void Reset()
        {
            _refEnumerator.Reset();
        }

        public bool MoveNext()
        {
            return _refEnumerator.MoveNext();
        }

        public void Dispose()
        {
            _refEnumerator.Dispose();
        }
    }
}
