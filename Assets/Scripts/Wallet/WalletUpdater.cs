using System;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Wallet.Contract;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using R3;

namespace LeaseExtension.Wallet
{
    internal class WalletUpdater : IDisposable
    {
        private readonly IDisposable _disposable;

        public WalletUpdater(
            IWalletModel model,
            ISubscriber<ToothPicked> toothSubscriber,
            ISubscriber<RestartRequested> resetSubscriber)
        {
            IDisposable d1 = resetSubscriber.Subscribe(_ => model.TopUp());
            IDisposable d2 = toothSubscriber.Subscribe(_ => model.Increment());
            _disposable = Disposable.Combine(d1, d2);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
