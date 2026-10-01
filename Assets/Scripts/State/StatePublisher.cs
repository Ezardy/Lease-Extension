using System;
using MessagePipe;
using Zenject;

namespace LeaseExtension.State
{
    public class StatePublisher<T>
        where T : Enum
    {
        private readonly IPublisher<T> _statePublisher;
        private readonly T _state;

        public StatePublisher(IPublisher<T> statePublisher, T state)
        {
            _statePublisher = statePublisher;
            _state = state;
        }

        public void Publish()
        {
            _statePublisher.Publish(_state);
        }

        public class Factory : PlaceholderFactory<T, StatePublisher<T>>
        {
        }
    }
}
