using System;
using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using MessagePipe;
using R3;
using UnityEngine.AddressableAssets;
using Zenject;

namespace LeaseExtension.SceneManagement.States
{
    [UsedImplicitly]
    internal class GameSceneState : SceneStateBase<GameSceneState>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Shop);
        private readonly ISubscriber<FocusedScene> _sceneRequestSubscriber;
        private readonly ShopSceneState.Factory _shopStateFactory;
        private IDisposable _disposable;

        public GameSceneState(
            ISceneContext context,
            [Inject(Id = FocusedScene.Game)] AssetReference scene,
            ISubscriber<FocusedScene> sceneRequestSubscriber,
            ShopSceneState.Factory shopStateFactory,
            ReactiveProperty<FocusedScene> statePublisher) : base(context, scene, statePublisher, FocusedScene.Game)
        {
            _sceneRequestSubscriber = sceneRequestSubscriber;
            _shopStateFactory = shopStateFactory;
        }

        public override void Start()
        {
            base.Start();
            _disposable = _sceneRequestSubscriber.Subscribe(_ => Context.State = _shopStateFactory.Create(), _filter);
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
            base.Dispose();
        }
    }
}