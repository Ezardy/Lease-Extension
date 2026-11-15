using System;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Save;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Save
{
    [CreateAssetMenu(fileName = "CosmeticsSave", menuName = "Scriptable Objects/Saves/Cosmetics")]
    internal class CosmeticsSave : SaveBase<Cosmetics>, IInitializable, IDisposable
    {
        private IAppliedCosmeticsModel _appliedCosmetics;
        private IDisposable _disposable;

        [Inject]
        public void Init(IAppliedCosmeticsModel appliedCosmetics)
        {
            _appliedCosmetics = appliedCosmetics;
        }

        public void Initialize()
        {
            Init();
            _disposable = _appliedCosmetics.IdChanged.Skip(1).Subscribe(h =>
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