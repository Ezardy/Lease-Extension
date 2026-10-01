using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Input.Contract;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace LeaseExtension.SceneManagment.States
{
    [UsedImplicitly]
    internal class WelcomeSceneState : AState<ISceneContext>
    {
        private readonly AssetReference _mainScene;
        private readonly StatePublisher<FocusedScene> _statePublisher;
        private readonly MainSceneState.Factory _mainFactory;
        private readonly CancellationTokenSource _cts = new();
        private readonly IDisposable _disposable;
        private bool _ready = false;
        private AsyncOperationHandle<SceneInstance> _handle;

        public WelcomeSceneState(
            ISceneContext context,
            StatePublisher<FocusedScene>.Factory publisherFactory,
            [Inject(Id = FocusedScene.Main)] AssetReference mainScene,
            ISubscriber<Tapped> tapSubscriber,
            MainSceneState.Factory mainFactory) : base(context)
        {
            _mainScene = mainScene;
            _statePublisher = publisherFactory.Create(FocusedScene.Welcome);
            _mainFactory = mainFactory;
            _disposable = tapSubscriber.Subscribe(_ =>
            {
                _ready = true;
                _disposable.Dispose();
            });
        }

        public override void Start()
        {
            _statePublisher.Publish();
            StartAsync().Forget();
        }

        public override void Dispose()
        {
            if (_handle.IsValid())
                _handle.Release();
            _cts.Cancel();
            _disposable?.Dispose();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        private async UniTaskVoid StartAsync()
        {
            AsyncOperationHandle<SceneInstance> sceneHandle = Addressables.LoadSceneAsync(
                _mainScene,
                LoadSceneMode.Additive);
            _handle = sceneHandle;
            await _handle.WithCancellation(_cts.Token);
            SetSceneGameObjectsActive(false);
            await UniTask.WaitUntil(() => _ready, cancellationToken: _cts.Token);
            SetSceneGameObjectsActive(true);
            _handle = new();
            Context.State = _mainFactory.Create(sceneHandle);
        }

        private void SetSceneGameObjectsActive(bool active)
        {
            foreach (GameObject go in _handle.Result.Scene.GetRootGameObjects())
                go.SetActive(active);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<WelcomeSceneState>
        {
        }
    }
}
