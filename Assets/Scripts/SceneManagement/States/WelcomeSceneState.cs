using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace LeaseExtension.SceneManagement.States
{
    [UsedImplicitly]
    internal class WelcomeSceneState : PoolablePublishingState<WelcomeSceneState, ISceneContext, FocusedScene>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Main);
        private readonly Scene _welcomeScene;
        private readonly AssetReference _mainScene;
        private readonly MainSceneState.Factory _mainFactory;
        private readonly CancellationTokenSource _cts = new();
        private readonly IDisposable _disposable;
        private bool _ready;
        private AsyncOperationHandle<SceneInstance> _handle;

        public WelcomeSceneState(
            ISceneContext context,
            Scene welcomeScene,
            ReactiveProperty<FocusedScene> statePublisher,
            ISubscriber<FocusedScene> requestSubscriber,
            [Inject(Id = FocusedScene.Main)] AssetReference mainScene,
            MainSceneState.Factory mainFactory) : base(context, statePublisher, FocusedScene.Welcome)
        {
            _welcomeScene = welcomeScene;
            _mainScene = mainScene;
            _mainFactory = mainFactory;
            _disposable = requestSubscriber.Subscribe(_ =>
            {
                _ready = true;
                _disposable.Dispose();
            }, _filter);
        }

        public override void Start()
        {
            base.Start();
            StartAsync().Forget();
        }

        public override void Dispose()
        {
            if (_handle.IsValid())
            {
                _handle.Release();
            }

            _cts.Cancel();
            _disposable?.Dispose();
            SceneManager.UnloadSceneAsync(_welcomeScene);
            base.Dispose();
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
            {
                go.SetActive(active);
            }
        }
    }
}