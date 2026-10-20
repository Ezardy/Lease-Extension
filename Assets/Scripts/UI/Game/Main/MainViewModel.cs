using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.UI.Contract;
using LeaseExtension.Wallet.Contract;
using R3;
using Unity.Properties;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    [UsedImplicitly]
    internal class MainViewModel : IMainViewModel
    {
        private readonly ReadOnlyReactiveProperty<GameplayState> _characterState;
        private readonly IRecordModel _recordModel;

        [CreateProperty]
        public StyleEnum<DisplayStyle> MainDisplayStyle => _characterState.CurrentValue == GameplayState.Idle
            ? DisplayStyle.Flex
            : DisplayStyle.None;

        [CreateProperty] public uint Record => _recordModel.Record;

        public MainViewModel(
            IMainView view,
            IWalletModel walletModel,
            ReadOnlyReactiveProperty<GameplayState> characterState,
            IRecordModel recordModel)
        {
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() =>
            {
                view.Root.dataSource = this;
                view.EarnedGroup.dataSource = walletModel;
            }).Forget();
            _characterState = characterState;
            _recordModel = recordModel;
        }
    }
}