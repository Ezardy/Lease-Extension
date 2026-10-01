using LeaseExtension.Wallet.Contract;
using R3;
using Unity.Properties;

namespace LeaseExtension.Wallet
{
    public class WalletModel : IWalletModel
    {
        private readonly ReactiveProperty<uint> _balance;
        private readonly ReactiveProperty<uint> _earned;

        [CreateProperty]
        public uint Balance => _balance.CurrentValue;

        [CreateProperty]
        public uint Earned => _earned.CurrentValue;
        public Observable<uint> BalanceChanged => _balance;
        public Observable<uint> EarnedChanged => _earned;

        public WalletModel(uint balance)
        {
            _balance = new(balance);
            _earned = new(0);
        }

        public void Increment()
        {
            _earned.Value += 1;
        }

        public void TopUp()
        {
            _balance.Value += _earned.Value;
            _earned.Value = 0;
        }

        public bool Withdraw(uint amount)
        {
            bool toWithdraw = amount <= Balance;
            if (toWithdraw)
                _balance.Value -= amount;
            return toWithdraw;
        }
    }
}
