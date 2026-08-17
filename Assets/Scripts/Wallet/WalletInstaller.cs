using UnityEngine;
using Zenject;

namespace Aniki.Wallet {
	[CreateAssetMenu(fileName = "WalletInstaller", menuName = "Installers/Wallet Installer")]
	internal class WalletInstaller : ScriptableObjectInstaller<WalletInstaller> {
		[SerializeField] private WalletSave	walletSave;

		public override void InstallBindings() {
			walletSave.Load();

			Container.BindInterfacesTo<WalletModel>().AsSingle().WithArguments(walletSave.Data.balance);
			Container.BindInterfacesTo<WalletSave>().FromInstance(walletSave);
			Container.QueueForInject(walletSave);
			Container.BindInterfacesTo<WalletViewModel>().AsSingle();
		}
	}
}