using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.UI.Contract;
using LeaseExtension.Wallet.Contract;
using R3;
using Unity.Properties;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Race
{
    [UsedImplicitly]
    internal class RaceViewModel : IRaceViewModel
    {
        private readonly ReadOnlyReactiveProperty<CharacterState> _characterState;
        private readonly IRecordModel _recordModel;
        private readonly IWalletModel _walletModel;

        [CreateProperty]
        public StyleEnum<DisplayStyle> GameDisplayStyle =>
            _characterState.CurrentValue != CharacterState.Idle && _characterState.CurrentValue != CharacterState.Over
                ? DisplayStyle.Flex
                : DisplayStyle.None;

        [CreateProperty] public uint BarsPassed => _recordModel.BarsPassed;

        [CreateProperty] public uint Earned => _walletModel.Earned;

        public RaceViewModel(
            IRaceView view,
            ReadOnlyReactiveProperty<CharacterState> characterState,
            IRecordModel recordModel,
            IWalletModel walletModel)
        {
            _characterState = characterState;
            _recordModel = recordModel;
            _walletModel = walletModel;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => view.Root.dataSource = this).Forget();
        }
    }
}