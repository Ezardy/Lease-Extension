using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.UI.Contract;
using LeaseExtension.Wallet.Contract;
using MessagePipe;
using R3;
using Unity.Properties;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Over
{
    [UsedImplicitly]
    internal class OverViewModel : IOverViewModel, IDisposable
    {
        private readonly IOverView _view;
        private readonly ReadOnlyReactiveProperty<CharacterState> _characterState;
        private readonly IRecordModel _recordModel;
        private readonly IWalletModel _walletModel;
        private readonly IPublisher<RestartRequested> _publisher;

        [CreateProperty]
        public StyleEnum<DisplayStyle> OverDisplayStyle => _characterState.CurrentValue == CharacterState.Over
            ? DisplayStyle.Flex
            : DisplayStyle.None;

        [CreateProperty]
        public StyleEnum<DisplayStyle> NewRecordDisplayStyle =>
            _recordModel.BarsPassed > _recordModel.Record ? DisplayStyle.Flex : DisplayStyle.None;

        [CreateProperty] public uint BarsPassed => _recordModel.BarsPassed;

        [CreateProperty] public uint Earned => _walletModel.Earned;

        public OverViewModel(
            IOverView view,
            ReadOnlyReactiveProperty<CharacterState> characterState,
            IRecordModel recordModel,
            IWalletModel walletModel,
            IPublisher<RestartRequested> publisher)
        {
            _view = view;
            _characterState = characterState;
            _recordModel = recordModel;
            _walletModel = walletModel;
            _publisher = publisher;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(Initialize).Forget();
        }

        public void Dispose()
        {
            if (_view.Root != null)
                _view.AcceptButton.clicked -= OnClick;
        }

        private void Initialize()
        {
            _view.Root.dataSource = this;
            _view.AcceptButton.clicked += OnClick;
        }

        private void OnClick()
        {
            _publisher.Publish(new());
        }
    }
}