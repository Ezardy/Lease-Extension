using R3;

namespace LeaseExtension.Wallet.Contract
{
    public interface IWalletModel
    {
        public uint Balance { get; }
        public uint Earned { get; }

        public Observable<uint> BalanceChanged { get; }
        public Observable<uint> EarnedChanged { get; }

        public void Increment();

        public void TopUp();

        public bool Withdraw(uint amount);
    }
}
