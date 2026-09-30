using Cysharp.Threading.Tasks;
using LeaseExtension.Gameplay.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.UI.Contract;
using LeaseExtension.Wallet.Contract;
using Unity.Properties;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Race
{
    [MovedFrom("Aniki.UI")]
    internal class RaceViewModel : IRaceViewModel
    {
        private readonly ICharacterModel _characterModel;
        private readonly IRecordModel _recordModel;
        private readonly IWalletModel _walletModel;

        [CreateProperty]
        public StyleEnum<DisplayStyle> GameDisplayStyle => _characterModel.State != CharacterState.Idle && _characterModel.State != CharacterState.Over ? DisplayStyle.Flex : DisplayStyle.None;

        [CreateProperty]
        public uint BarsPassed => _recordModel.BarsPassed;

        [CreateProperty]
        public uint Earned => _walletModel.Earned;

        public RaceViewModel(
            IRaceView view,
            ICharacterModel characterModel,
            IRecordModel recordModel,
            IWalletModel walletModel)
        {
            _characterModel = characterModel;
            _recordModel = recordModel;
            _walletModel = walletModel;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => view.Root.dataSource = this).Forget();
        }
    }
}
