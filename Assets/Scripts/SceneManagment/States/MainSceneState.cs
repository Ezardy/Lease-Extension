using Aniki.State;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Aniki.SceneManagment {
	internal class MainSceneState : APoolablePublishingState<AsyncOperationHandle<SceneInstance>, MainSceneState, ISceneContext, FocusedScene> {
		private AsyncOperationHandle<SceneInstance>	sceneHandle;

		public MainSceneState(ISceneContext context,
			StatePublisher<FocusedScene>.Factory publisherFactory)
			: base(context, publisherFactory.Create(FocusedScene.MAIN)) { }

		public override void	OnSpawned(AsyncOperationHandle<SceneInstance> sceneHandle, IMemoryPool pool) {
			base.OnSpawned(sceneHandle, pool);
			this.sceneHandle = sceneHandle;
		}

		public override void	Dispose() {
			sceneHandle.Release();
			sceneHandle = new();
			base.Dispose();
		}
	}
}
