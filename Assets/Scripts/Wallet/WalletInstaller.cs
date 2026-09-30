using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Wallet
{
    [CreateAssetMenu(fileName = "WalletInstaller", menuName = "Installers/Wallet Installer")]
    [MovedFrom("Aniki.Wallet")]
    internal class WalletInstaller : ScriptableObjectInstaller<WalletInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("walletSave")]
        private WalletSave _walletSave;

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
