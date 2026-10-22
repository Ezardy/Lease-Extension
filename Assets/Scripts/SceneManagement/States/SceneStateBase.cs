using System.Threading;
using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LeaseExtension.SceneManagement.States
{
    internal abstract class SceneStateBase<T> : AsyncPoolablePublishingState<SceneStateBase<T>, ISceneContext, FocusedScene>, IAsyncLoadable
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

        protected override void StartLoad()
        {
            _sceneHandle = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
            _cts = new();
            _token = _cts.Token;
        }

        public UniTask LoadAsync()
        {
            return _sceneHandle.ToUniTask(cancellationToken: _token).SuppressCancellationThrow();
        }

        public override void Dispose()
        {
            Addressables.UnloadSceneAsync(_sceneHandle);
            _cts.Cancel();
            _cts.Dispose();
            base.Dispose();
        }
    }
}