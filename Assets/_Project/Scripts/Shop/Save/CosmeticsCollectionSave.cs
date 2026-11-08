using System;
using LeaseExtension.Save;
using LeaseExtension.Shop.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Shop.Save
{
    [CreateAssetMenu(fileName = "CosmeticsCollectionSave", menuName = "Scriptable Objects/Saves/Cosmetics Collection")]
    internal class CosmeticsCollectionSave : SaveBase<CosmeticsCollection>, IInitializable, IDisposable
    {
        private ITargetProductListModel _model;
        private IDisposable _disposable;

        [Inject]
        public void Init(ITargetProductListModel cosmetics)
        {
            _model = cosmetics;
        }

        public void Initialize()
        {
            Init();
            _disposable = _model.ProductOwned.Subscribe(e =>
            {
                SaveData.IdCollection.Add(e.Value);
                Save();
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}