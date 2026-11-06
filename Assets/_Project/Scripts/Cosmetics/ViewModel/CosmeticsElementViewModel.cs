using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.Model;
using R3;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace LeaseExtension.Cosmetics.ViewModel
{
    [UsedImplicitly]
    internal class CosmeticsElementViewModel : ICosmeticsElementViewModel, IInitializable, IDisposable
    {
        private readonly ICosmeticsElementModel _cosmeticsModel;
        private readonly ICosmeticsElementView _cosmeticsView;
        private AsyncOperationHandle<CosmeticsItemModel> _handle;
        private IDisposable _disposable;
        private bool _isDisposed;
        private int _requestVersion;

        public CosmeticsElementViewModel(
            ICosmeticsElementModel cosmeticsModel,
            ICosmeticsElementView cosmeticsView)
        {
            _cosmeticsModel = cosmeticsModel;
            _cosmeticsView = cosmeticsView;
        }

        public void Initialize()
        {
            _disposable = _cosmeticsModel.IdChanged.Subscribe(h => Set(h).Forget());
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

        private async UniTask Set(string id)
        {
            int requestVersion = ++_requestVersion;
            if (!_isDisposed)
            {
                AsyncOperationHandle<CosmeticsItemModel> handle = Addressables.LoadAssetAsync<CosmeticsItemModel>(id);
                bool isRetained = false;
                try
                {
                    ICosmeticsItemModel item = await handle.ToUniTask();
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
