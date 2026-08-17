using Aniki.Save;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.Wallet {
	[CreateAssetMenu(fileName = "WalletSave", menuName = "Scriptable Objects/Saves/Wallet")]
	internal class WalletSave : ASave<WalletSave.Wallet, IWalletModel>, IInitializable, IDisposable {
		[Serializable]
		public struct Wallet {
			public uint	balance;
		}

		private IWalletModel	wallet;
		private IDisposable		disposable;

		public void	Initialize() {
			Init();
			disposable = wallet.BalanceChanged.Subscribe(b => {
				data.balance = b;
				Save();
			});
		}

		public void	Dispose() {
			disposable.Dispose();
		}

		[Inject]
		public void	Init(IWalletModel wallet) {
			this.wallet = wallet;
		}
	}
}
