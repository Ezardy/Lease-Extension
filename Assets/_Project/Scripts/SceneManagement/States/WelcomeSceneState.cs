using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using LeaseExtension.State.Contract;
using MessagePipe;
using R3;
using UnityEngine.SceneManagement;

namespace LeaseExtension.SceneManagement.States
{
    [UsedImplicitly]
    internal class WelcomeSceneState : PoolablePublishingState<WelcomeSceneState, ISceneContext, FocusedScene>
    {
        private static readonly FocusedSceneFilter _filter = new(FocusedScene.Game);
        private readonly Scene _welcomeScene;
        private readonly GameSceneState.Factory _mainFactory;
        private readonly CancellationTokenSource _cts = new();
        private readonly CancellationToken _token;
        private readonly IDisposable _disposable;
        private IState _awaitingState;
        private bool _ready;

        public WelcomeSceneState(
            ISceneContext context,
            Scene welcomeScene,
            ReactiveProperty<FocusedScene> statePublisher,
            ISubscriber<FocusedScene> requestSubscriber,
            GameSceneState.Factory mainFactory) : base(context, statePublisher, FocusedScene.Welcome)
        {
            _welcomeScene = welcomeScene;
            _mainFactory = mainFactory;
            _token = _cts.Token;
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
            _cts.Cancel();
            _awaitingState?.Dispose();
            _disposable?.Dispose();
            SceneManager.UnloadSceneAsync(_welcomeScene);
            base.Dispose();
        }

        private async UniTaskVoid StartAsync()
        {
            _awaitingState = await _mainFactory.CreateAsync();
            await UniTask.WaitUntil(() => _ready, cancellationToken: _token);
            IState nextState = _awaitingState;
            _awaitingState = null;
            Context.State = nextState;
        }
    }
}