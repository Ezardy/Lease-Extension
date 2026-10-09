using System;
using MessagePipe;

namespace LeaseExtension.Common.Utilities
{
    public class EqualityFilter<T> : MessageHandlerFilter<T> where T : struct
    {
        private readonly T _sample;

        public EqualityFilter(T sample)
        {
            _sample = sample;
        }

        public override void Handle(T message, Action<T> next)
        {
            if (_sample.Equals(message))
                next(message);
        }
    }
}
