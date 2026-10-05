using System;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Save;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics
{
    [CreateAssetMenu(fileName = "CosmeticsCollectionSave", menuName = "Scriptable Objects/Saves/Cosmetics Collection")]
    internal class CosmeticsCollectionSave : ASave<CosmeticsCollection>, IInitializable, IDisposable
    {
        private ICosmeticsElementModel _cosmetics;
        private IDisposable _disposable;

        [Inject]
        public void Init(ICosmeticsElementModel cosmetics)
        {
            _cosmetics = cosmetics;
        }

        public void Initialize()
        {
            Init();
            _disposable = _cosmetics.IdCollectionChanged.Skip(1).Subscribe(e =>
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
