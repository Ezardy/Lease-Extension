using System;
using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using R3;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace LeaseExtension.SceneManagement.States
{
    [UsedImplicitly]
    internal class GameSceneState : PoolablePublishingState<AsyncOperationHandle<SceneInstance>, GameSceneState,
        ISceneContext, FocusedScene>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Shop);
        private readonly ISubscriber<FocusedScene> _sceneRequestSubscriber;
        private readonly ShopSceneState.Factory _shopStateFactory;
        private AsyncOperationHandle<SceneInstance> _sceneHandle;
        private IDisposable _disposable;

        public GameSceneState(
            ISceneContext context,
            ISubscriber<FocusedScene> sceneRequestSubscriber,
            ShopSceneState.Factory shopStateFactory,
            ReactiveProperty<FocusedScene> statePublisher) : base(context, statePublisher, FocusedScene.Game)
        {
            _sceneRequestSubscriber = sceneRequestSubscriber;
            _shopStateFactory = shopStateFactory;
        }

        public override void OnSpawned(AsyncOperationHandle<SceneInstance> sceneHandle, IMemoryPool pool)
        {
            base.OnSpawned(sceneHandle, pool);
            _sceneHandle = sceneHandle;
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
            _sceneHandle.Release();
            _sceneHandle = new();
            base.Dispose();
        }
    }
}