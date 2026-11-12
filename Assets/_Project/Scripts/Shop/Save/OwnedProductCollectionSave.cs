using System;
using LeaseExtension.Save;
using LeaseExtension.Shop.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Shop.Save
{
    [CreateAssetMenu(fileName = "CosmeticsCollectionSave", menuName = "Scriptable Objects/Saves/Cosmetics Collection")]
    internal class OwnedProductCollectionSave : SaveBase<OwnedProductCollection>, IInitializable, IDisposable
    {
        private ITargetProductListModel _model;
        private IDisposable _disposable;

        public void Init(ITargetProductListModel model)
        {
            _model = model;
        }

        public void Initialize()
        {
            SaveData.Ids ??= new();
            Init();
            _disposable = _model.ProductOwned.Subscribe(e =>
            {
                SaveData.Ids.Add(e.Value);
                Save();
            });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }
    }
}