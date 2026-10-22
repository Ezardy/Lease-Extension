using System;
using LeaseExtension.SceneManagement.Contract.Message;
using MessagePipe;
using R3;
using UnityEngine.AddressableAssets;
using Zenject;

namespace LeaseExtension.SceneManagement.States
{
    internal class ShopSceneState : SceneStateBase<ShopSceneState>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Game);
        private readonly ISubscriber<FocusedScene> _sceneRequestSubscriber;
        private readonly GameSceneState.Factory _gameStateFactory;
        private IDisposable _disposable;
        
        public ShopSceneState(ISceneContext context, [Inject(Id = FocusedScene.Shop)] AssetReference shopScene, ReactiveProperty<FocusedScene> statePublisher, ISubscriber<FocusedScene> sceneRequestSubscriber, GameSceneState.Factory gameStateFactory) : base(context, shopScene,
            statePublisher, FocusedScene.Shop)
        {
            _sceneRequestSubscriber = sceneRequestSubscriber;
            _gameStateFactory = gameStateFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _sceneRequestSubscriber.Subscribe(_ => Context.State = _gameStateFactory.Create(), _filter);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
            base.Dispose();
        }
    }
}