using Aniki.UI;
using MessagePipe;
using R3;
using System;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Entities.Message;

namespace Aniki.Wallet {
	internal class WalletUpdater : IDisposable {
		private readonly IDisposable	disposable;

		public WalletUpdater(IWalletModel model,
			ISubscriber<ToothPicked> toothSubscriber,
			ISubscriber<RestartRequested> resetSubscriber) {
			IDisposable	d1 = resetSubscriber.Subscribe(_ => model.TopUp());
			IDisposable	d2 = toothSubscriber.Subscribe(_ => model.Increment());
			disposable = Disposable.Combine(d1, d2);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
