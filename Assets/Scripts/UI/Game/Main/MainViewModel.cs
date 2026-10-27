using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Record.Contract;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.Wallet.Contract;
using MessagePipe;
using R3;
using Unity.Properties;
using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    [UsedImplicitly]
    internal class MainViewModel : IMainViewModel, IDisposable
    {
        private readonly ReadOnlyReactiveProperty<GameplayState> _characterState;
        private readonly IRecordModel _recordModel;
        private readonly IMainView _view;
        private readonly IPublisher<FocusedScene> _sceneRequestPublisher;

        [CreateProperty]
        public StyleEnum<DisplayStyle> MainDisplayStyle => _characterState.CurrentValue == GameplayState.Idle
            ? DisplayStyle.Flex
            : DisplayStyle.None;

        [CreateProperty] public uint Record => _recordModel.Record;

        public MainViewModel(
            IMainView view,
            IWalletModel walletModel,
            ReadOnlyReactiveProperty<GameplayState> characterState,
            IRecordModel recordModel,
            IPublisher<FocusedScene> sceneRequestPublisher)
        {
            _characterState = characterState;
            _recordModel = recordModel;
            _view = view;
            _sceneRequestPublisher = sceneRequestPublisher;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() =>
            {
                view.Root.dataSource = this;
                view.Store.clicked += OnStoreButtonClicked;
            }).Forget();
        }

        public void Dispose()
        {
            if (_view.Root is { dataSource: not null })
            {
                _view.Store.clicked -= OnStoreButtonClicked;
            }
        }

        private void OnStoreButtonClicked()
        {
            _sceneRequestPublisher.Publish(FocusedScene.Shop);
        }
    }
}