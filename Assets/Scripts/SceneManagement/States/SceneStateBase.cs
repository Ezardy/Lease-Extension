using System.Threading;
using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using LeaseExtension.Common.Utilities;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LeaseExtension.SceneManagement.States
{
    internal abstract class SceneStateBase<T> : AsyncPoolablePublishingState<T, ISceneContext, FocusedScene>, IAsyncLoadable
        where T : SceneStateBase<T>
    {
        private readonly AssetReference _scene;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        protected SceneStateBase(ISceneContext context, AssetReference scene,
            ReactiveProperty<FocusedScene> statePublisher, FocusedScene state) : base(context, statePublisher, state)
        {
            _scene = scene;
        }

        protected async UniTask SwitchState<TSceneState>(AsyncPlaceholderFactory<TSceneState> sceneFactory) where TSceneState : SceneStateBase<TSceneState>
        {
            Context.State = await sceneFactory.CreateAsync();
        }

        protected override void StartLoad()
        {
            _sceneHandle = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
            _cts = new();
            _token = _cts.Token;
        }

        public async UniTask<bool> LoadAsync()
        {
            await _sceneHandle.WithCancellation(_token).SuppressCancellationThrow();
            bool failed = _token.IsCancellationRequested;
            if (failed)
            {
                Dispose();
            }
            _cts.Dispose();
            _cts = null;
            return !failed;
        }

        public override void Dispose()
        {
            Addressables.UnloadSceneAsync(_sceneHandle);
            _cts?.Cancel();
            base.Dispose();
        }
    }
}
