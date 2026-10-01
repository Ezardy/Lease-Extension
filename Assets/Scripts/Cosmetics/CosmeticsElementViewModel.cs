using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LeaseExtension.Common.Utilities;
using LeaseExtension.Cosmetics.Contract;
using R3;
using Zenject;

namespace LeaseExtension.Cosmetics
{
    [UsedImplicitly]
    internal class CosmeticsElementViewModel<T> : ICosmeticsElementViewModel, ITyped<T>, IInitializable, IDisposable where T : class
    {
        private readonly ICosmeticsItemModelDatabase _hairstyleDatabase;
        private readonly ICosmeticsElementModel _cosmeticsModel;
        private readonly ICosmeticsElementView _cosmeticsView;
        private ICosmeticsSpriteHandles _handles;
        private IDisposable _disposable;

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
            _cosmeticsView.Set(null, null);
            _handles.Release();
            _disposable?.Dispose();
        }

        private async UniTaskVoid Set(string id)
        {
            _handles?.Release();
            _handles = _hairstyleDatabase.GetCosmeticsItemModel(id).GetSpriteHandles();
            await _handles.LoadTask();
            _cosmeticsView.Set(_handles.Back, _handles.Front);
        }
    }
}
