using JetBrains.Annotations;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State;
using R3;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace LeaseExtension.SceneManagment.States
{
    [UsedImplicitly]
    internal class MainSceneState : APoolablePublishingState<AsyncOperationHandle<SceneInstance>, MainSceneState, ISceneContext, FocusedScene>
    {
        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        public MainSceneState(
            ISceneContext context,
            ReactiveProperty<FocusedScene> statePublisher) : base(context, statePublisher, FocusedScene.Main)
        {
        }

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
