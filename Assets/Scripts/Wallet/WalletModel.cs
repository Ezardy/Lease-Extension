using R3;
using Unity.Properties;

namespace Aniki.Wallet {
	public class WalletModel : IWalletModel {
		private readonly ReactiveProperty<uint>	balance;
		private readonly ReactiveProperty<uint>	earned;

		public WalletModel(uint balance) {
			this.balance = new(balance);
			earned = new(0);
		}

		[CreateProperty] public uint	Balance => balance.CurrentValue;

		[CreateProperty] public uint	Earned => earned.CurrentValue;

		public void	Increment() {
			earned.Value += 1;
		}

		public void	TopUp() {
			balance.Value += earned.Value;
			earned.Value = 0;
		}

		public bool	Withdraw(uint amount) {
			bool	toWithdraw = amount <= Balance;
			if (toWithdraw)
				balance.Value -= amount;
			return toWithdraw;
		}

		public Observable<uint>	BalanceChanged => balance;
		public Observable<uint> EarnedChanged => earned;
	}
}
