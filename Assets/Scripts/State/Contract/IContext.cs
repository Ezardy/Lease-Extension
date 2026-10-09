using System;

namespace LeaseExtension.State.Contract
{
    public interface IContext : IDisposable
    {
        public IState State { get; set; }
    }
}
