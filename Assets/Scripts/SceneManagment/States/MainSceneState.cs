using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace LeaseExtension.SceneManagment.States
{
    internal class MainSceneState : APoolablePublishingState<AsyncOperationHandle<SceneInstance>, MainSceneState, ISceneContext, FocusedScene>
    {
        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        public MainSceneState(
            ISceneContext context,
            StatePublisher<FocusedScene>.Factory publisherFactory) : base(
            context,
            publisherFactory.Create(FocusedScene.Main))
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
