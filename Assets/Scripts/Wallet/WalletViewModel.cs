using Aniki.UI;
using Aniki.World;
using MessagePipe;
using R3;
using System;

namespace Aniki.Wallet {
	internal class WalletViewModel : IDisposable {
		private readonly IDisposable	disposable;

		public WalletViewModel(IWalletModel model,
			ISubscriber<ToothCollisionMessage> toothSubscriber,
			ISubscriber<AcceptSentenceMessage> resetSubscriber) {
			IDisposable	d1 = resetSubscriber.Subscribe(_ => model.TopUp());
			IDisposable	d2 = toothSubscriber.Subscribe(_ => model.Increment());
			disposable = Disposable.Combine(d1, d2);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
