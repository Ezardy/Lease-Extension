using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.UI.Contract;
using LeaseExtension.Wallet.Contract;
using Unity.Properties;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    [UsedImplicitly]
    internal class MainViewModel : IMainViewModel
    {
        private readonly ICharacterModel _characterModel;
        private readonly IRecordModel _recordModel;

        [CreateProperty]
        public StyleEnum<DisplayStyle> MainDisplayStyle => _characterModel.State == CharacterState.Idle ? DisplayStyle.Flex : DisplayStyle.None;

        [CreateProperty]
        public uint Record => _recordModel.Record;

        public MainViewModel(
            IMainView view,
            IWalletModel walletModel,
            ICharacterModel characterModel,
            IRecordModel recordModel)
        {
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() =>
            {
                view.Root.dataSource = this;
                view.EarnedGroup.dataSource = walletModel;
            }).Forget();
            _characterModel = characterModel;
            _recordModel = recordModel;
        }
    }
}
