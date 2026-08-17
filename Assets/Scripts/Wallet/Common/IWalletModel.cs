using R3;

namespace Aniki.Wallet {
	public interface IWalletModel {
		public uint	Balance { get; }
		public uint	Earned { get; }
		public void	Increment();
		public void	TopUp();
		public bool	Withdraw(uint amount);

		public Observable<uint>	BalanceChanged { get; }
		public Observable<uint> EarnedChanged { get; }
	}
}
