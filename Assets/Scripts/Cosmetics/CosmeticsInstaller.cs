using System;
using LeaseExtension.Common.Utilities;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.Contract.Target;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics
{
    [CreateAssetMenu(fileName = "CosmeticsInstaller", menuName = "Installers/Cosmetics Installer")]
    internal class CosmeticsInstaller : ScriptableObjectInstaller<CosmeticsInstaller>
    {
        [SerializeField] private CosmeticsElementModelDependencies _babHairstyleModelDependencies;
        [SerializeField] private CosmeticsElementModelDependencies _pupHairstyleModelDependencies;

        public override void InstallBindings()
        {
            Install<BabHairstyle, BabHairstyle.ElementModel, BabHairstyle.Database>(
                _babHairstyleModelDependencies.CosmeticsSave,
                _babHairstyleModelDependencies.CosmeticsCollectionSave,
                _babHairstyleModelDependencies.Database.I);
            Install<PupHairstyle, PupHairstyle.ElementModel, PupHairstyle.Database>(
                _pupHairstyleModelDependencies.CosmeticsSave,
                _pupHairstyleModelDependencies.CosmeticsCollectionSave,
                _pupHairstyleModelDependencies.Database.I);
        }

        private void Install<T, M, D>(
            CosmeticsSave save,
            CosmeticsCollectionSave collectionSave,
            ICosmeticsItemModelDatabase database)
            where M : ICosmeticsElementModel where D : ICosmeticsItemModelDatabase where T : class
        {
            Container.Bind<ICosmeticsElementModel>().FromSubContainerResolve().ByMethod(
                c => InstallHairstyle<BabHairstyle.ElementModel>(c, save, collectionSave)).AsCached().WithTypeOrAny<T>();
            Container.Bind<ICosmeticsItemModelDatabase>().FromSubContainerResolve().ByMethod(
                c => InstallDatabase<BabHairstyle.Database>(c, database)).AsCached().WithTypeOrAny<T>();
        }

        private void InstallHairstyle<M>(
            DiContainer container,
            CosmeticsSave cosmeticsSave,
            CosmeticsCollectionSave cosmeticsCollectionSave)
            where M : ICosmeticsElementModel
        {
            cosmeticsSave.Load();
            cosmeticsCollectionSave.Load();
            container.BindInterfacesTo<CosmeticsElementModel>().AsSingle().WithArguments(
                cosmeticsSave.Data.Id,
                cosmeticsCollectionSave.Data.IdCollection);
            container.BindInterfacesTo<CosmeticsSave>().FromInstance(cosmeticsSave);
            container.BindInterfacesTo<CosmeticsCollectionSave>().FromInstance(cosmeticsCollectionSave);
            container.QueueForInject(cosmeticsSave);
            container.QueueForInject(cosmeticsCollectionSave);
            container.Decorate<ICosmeticsElementModel>().With<M>();
        }

        private void InstallDatabase<D>(DiContainer container, ICosmeticsItemModelDatabase database)
            where D : ICosmeticsItemModelDatabase
        {
            container.BindInstance(database);
            container.Decorate<ICosmeticsItemModelDatabase>().With<D>();
        }

        [Serializable]
        private class CosmeticsElementModelDependencies
        {
            public CosmeticsSave CosmeticsSave;
            public CosmeticsCollectionSave CosmeticsCollectionSave;
            public Ref<ICosmeticsItemModelDatabase> Database;
        }
    }
}
