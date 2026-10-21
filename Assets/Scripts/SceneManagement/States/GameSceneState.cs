using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
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
        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        public GameSceneState(
            ISceneContext context,
            ReactiveProperty<FocusedScene> statePublisher) : base(context, statePublisher, FocusedScene.Game)
        {}

        public override void OnSpawned(AsyncOperationHandle<SceneInstance> sceneHandle, IMemoryPool pool)
        {
            base.OnSpawned(sceneHandle, pool);
            _sceneHandle = sceneHandle;
        }

        public override void Dispose()
        {
            _sceneHandle.Release();
            _sceneHandle = new();
            base.Dispose();
        }
    }
}