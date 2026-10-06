using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using R3;
using Zenject;

namespace LeaseExtension.Cosmetics.ViewModel
{
    [UsedImplicitly]
    internal class CosmeticsElementViewModel : ICosmeticsElementViewModel, IInitializable, IDisposable
    {
        private readonly ICosmeticsItemModelDatabase _hairstyleDatabase;
        private readonly ICosmeticsElementModel _cosmeticsModel;
        private readonly ICosmeticsElementView _cosmeticsView;
        private ICosmeticsSpriteHandles _handles;
        private IDisposable _disposable;
        private bool _isDisposed;

        public CosmeticsElementViewModel(
            ICosmeticsItemModelDatabase database,
            ICosmeticsElementModel cosmeticsModel,
            ICosmeticsElementView cosmeticsView)
        {
            _hairstyleDatabase = database;
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
            _handles?.Release();
        }

        private async UniTaskVoid Set(string id)
        {
            _handles?.Release();
            _handles = _hairstyleDatabase.GetCosmeticsItemModel(id).GetSpriteHandles();
            ICosmeticsSpriteHandles handles = _handles;
            await handles.LoadTask();
            if (!_isDisposed && ReferenceEquals(handles, _handles))
                _cosmeticsView.Set(handles.Back, handles.Front);
        }
    }
}
