using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.View;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace LeaseExtension.Cosmetics.ViewModel
{
    [UsedImplicitly]
    internal class AppliedCosmeticsViewModel : ICosmeticsElementViewModel, IInitializable, IDisposable
    {
        private readonly IAppliedCosmeticsModel _appliedCosmeticsModel;
        private readonly ICosmeticsElementView _cosmeticsView;
        private AsyncOperationHandle<ICosmeticsModel> _handle;
        private IDisposable _disposable;
        private bool _isDisposed;
        private int _requestVersion;

        public AppliedCosmeticsViewModel(
            IAppliedCosmeticsModel appliedCosmeticsModel,
            ICosmeticsElementView cosmeticsView)
        {
            _appliedCosmeticsModel = appliedCosmeticsModel;
            _cosmeticsView = cosmeticsView;
        }

        public void Initialize()
        {
            _disposable = _appliedCosmeticsModel.IdChanged.Subscribe(h => Set(h).Forget());
        }

        public void Dispose()
        {
            _isDisposed = true;
            _disposable?.Dispose();
            _cosmeticsView.Set(null, null);
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
                _handle = default;
            }
        }

        private async UniTaskVoid Set(string id)
        {
            int requestVersion = ++_requestVersion;
            if (!_isDisposed)
            {
                AsyncOperationHandle<ICosmeticsModel> handle = Addressables.LoadAssetAsync<ICosmeticsModel>(id);
                bool isRetained = false;
                try
                {
                    ICosmeticsModel item = await handle.ToUniTask();
                    if (!_isDisposed && requestVersion == _requestVersion)
                    {
                        _cosmeticsView.Set(item.Back, item.Front);
                        if (_handle.IsValid())
                        {
                            Addressables.Release(_handle);
                        }

                        _handle = handle;
                        isRetained = true;
                    }
                }
                finally
                {
                    if (!isRetained && handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }
                }
            }
        }
    }
}