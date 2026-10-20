using UnityEngine;
using Zenject;

namespace LeaseExtension.Wallet.Installer
{
    [CreateAssetMenu(fileName = "WalletUpdaterInstaller", menuName = "Installers/Wallet/Updater")]
    internal class WalletGameInstaller : ScriptableObjectInstaller<WalletGameInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<WalletUpdater>().AsSingle();
        }
    }
}
