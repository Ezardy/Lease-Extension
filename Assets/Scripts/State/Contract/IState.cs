using System;

namespace LeaseExtension.State.Contract
{
    public interface IState : IDisposable
    {
        public void Start();

        public void Update();
    }
}
