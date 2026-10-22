using System;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Zenject;

namespace LeaseExtension.SceneManagement.States
{
    internal class ShopSceneState : PoolablePublishingState<ShopSceneState, ISceneContext, FocusedScene>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Game);
        private readonly AssetReference _shopScene;
        private readonly ISubscriber<FocusedScene> _sceneRequestSubscriber;
        private readonly GameSceneState.Factory _gameStateFactory;
        private AsyncOperationHandle<SceneHandle> _sceneHandle;
        private IDisposable _disposable;
        
        public ShopSceneState(ISceneContext context, [Inject(Id = FocusedScene.Shop)] AssetReference shopScene, ReactiveProperty<FocusedScene> statePublisher, ISubscriber<FocusedScene> sceneRequestSubscriber, GameSceneState.Factory gameStateFactory) : base(context,
            statePublisher, FocusedScene.Shop)
        {
            _shopScene = shopScene;
            _sceneRequestSubscriber = sceneRequestSubscriber;
            _gameStateFactory = gameStateFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _sceneRequestSubscriber.Subscribe(_ => Context.State = _gameStateFactory.Create(), _filter);
            
        }
    }
}