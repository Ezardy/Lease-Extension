using System;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Save;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Save
{
    [CreateAssetMenu(fileName = "CosmeticsSave", menuName = "Scriptable Objects/Saves/Cosmetics")]
    internal class CosmeticsSave : ASave<Save.Cosmetics>, IInitializable, IDisposable
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
            _disposable = _cosmetics.IdChanged.Skip(1).Subscribe(h =>
            {
                SaveData.Id = h;
                Save();
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
