using System;
using LeaseExtension.Save;
using LeaseExtension.Wallet.Contract;
using R3;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.Wallet
{
    [CreateAssetMenu(fileName = "WalletSave", menuName = "Scriptable Objects/Saves/Wallet")]
    [MovedFrom("Aniki.Wallet")]
    internal class WalletSave : ASave<Wallet, IWalletModel>, IInitializable, IDisposable
    {
        private IWalletModel _wallet;
        private IDisposable _disposable;

        public void Initialize()
        {
            Init();
            _disposable = _wallet.BalanceChanged.Subscribe(b =>
            {
                SaveData.Balance = b;
                Save();
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        [Inject]
        public void Init(IWalletModel wallet)
        {
            _wallet = wallet;
        }
    }
}
