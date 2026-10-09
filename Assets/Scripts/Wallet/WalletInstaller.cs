using UnityEngine;
using Zenject;

namespace LeaseExtension.Wallet
{
    [CreateAssetMenu(fileName = "WalletInstaller", menuName = "Installers/Wallet Installer")]
    internal class WalletInstaller : ScriptableObjectInstaller<WalletInstaller>
    {
        [SerializeField] private WalletSave _walletSave;

        public override void InstallBindings()
        {
            _walletSave.Load();
            Container.BindInterfacesTo<WalletModel>().AsSingle().WithArguments(_walletSave.Data.Balance);
            Container.BindInterfacesTo<WalletSave>().FromInstance(_walletSave);
            Container.QueueForInject(_walletSave);
            Container.BindInterfacesTo<WalletUpdater>().AsSingle();
        }
    }
}
