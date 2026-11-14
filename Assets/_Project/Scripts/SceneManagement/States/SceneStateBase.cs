using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using LeaseExtension.Common.Factory;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.State;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LeaseExtension.SceneManagement.States
{
    internal abstract class SceneStateBase<T> : AsyncPoolablePublishingState<T, ISceneContext, FocusedScene>,
        IAsyncLoadable
        where T : SceneStateBase<T>
    {
        private readonly AssetReference _scene;
        private readonly IReadOnlyCollection<Object> _prefabs;
        private readonly PrefabFactory.Factory _prefabFactory;
        private readonly List<GameObject> _instances;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        protected SceneStateBase(ISceneContext context, AssetReference scene,
            ReactiveProperty<FocusedScene> statePublisher, FocusedScene state, IReadOnlyCollection<Object> prefabs, PrefabFactory.Factory prefabFactory) : base(context, statePublisher, state)
        {
            _scene = scene;
            _prefabs = prefabs;
            _prefabFactory = prefabFactory;
            _instances = _prefabs == null ? null : new(_prefabs.Count);
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

        public override void Start()
        {
            base.Start();
            if (_prefabs != null)
            {
                foreach (Object prefab in _prefabs)
                {
                    _instances.Add(_prefabFactory.Create(prefab));
                }
            }
        }

        public override void Dispose()
        {
            _instances?.Clear();
            Addressables.UnloadSceneAsync(_sceneHandle);
            _cts?.Cancel();
            base.Dispose();
        }

        protected async UniTask SwitchState<TSceneState>(AsyncPlaceholderFactory<TSceneState> sceneFactory)
            where TSceneState : SceneStateBase<TSceneState>
        {
            if (_instances != null)
            {
                foreach (GameObject instance in _instances)
                {
                    Object.Destroy(instance);
                }
                _instances.Clear();
            }
            Context.State = await sceneFactory.CreateAsync();
        }

        protected override void StartLoad()
        {
            _sceneHandle = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
            _cts = new();
            _token = _cts.Token;
        }
    }
}