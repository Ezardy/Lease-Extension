using Aniki.Input;
using Aniki.State;
using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Aniki.SceneManagment {
	internal class WelcomeSceneState : AState<ISceneContext> {
		private readonly AssetReference					mainScene;
		private readonly StatePublisher<FocusedScene>	statePublisher;
		private readonly MainSceneState.Factory			mainFactory;
		private readonly CancellationTokenSource		cts = new();
		private readonly IDisposable					disposable;

		private bool								ready = false;
		private AsyncOperationHandle<SceneInstance>	handle;

		public WelcomeSceneState(ISceneContext context,
			StatePublisher<FocusedScene>.Factory publisherFactory,
			[Inject(Id = FocusedScene.MAIN)] AssetReference mainScene,
			ISubscriber<TapInputMessage> tapSubscriber,
			MainSceneState.Factory mainFactory)
			: base(context) {
			this.mainScene = mainScene;
			statePublisher = publisherFactory.Create(FocusedScene.WELCOME);
			this.mainFactory = mainFactory;
			disposable = tapSubscriber.Subscribe(_ => {
				ready = true;
				disposable.Dispose();
			});
		}

		public override void	Start() {
			statePublisher.Publish();
			StartAsync().Forget();
		}

		private async UniTaskVoid	StartAsync() {
			AsyncOperationHandle<SceneInstance>	sceneHandle = Addressables.LoadSceneAsync(mainScene, LoadSceneMode.Additive);

			handle = sceneHandle;
			await handle.WithCancellation(cts.Token);
			SetSceneGameObjectsActive(false);
			await UniTask.WaitUntil(() => ready, cancellationToken: cts.Token);
			SetSceneGameObjectsActive(true);
			handle = new();
			context.State = mainFactory.Create(sceneHandle);
		}

		private void	SetSceneGameObjectsActive(bool active) {
			foreach (GameObject go in handle.Result.Scene.GetRootGameObjects()) {
				go.SetActive(active);
			}
		}

		public override void	Dispose() {
			if (handle.IsValid())
				handle.Release();
			cts.Cancel();
			disposable?.Dispose();
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		}

		public class Factory : PlaceholderFactory<WelcomeSceneState> { }
	}
}
