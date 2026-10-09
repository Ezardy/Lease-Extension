using System;
using LeaseExtension.Common.Utilities;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.Model;
using LeaseExtension.Cosmetics.Save;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Installer
{
    [CreateAssetMenu(fileName = "CosmeticsInstaller", menuName = "Installers/Cosmetics Installer")]
    internal partial class CosmeticsInstaller : ScriptableObjectInstaller<CosmeticsInstaller>
    {
        [SerializeField] private CosmeticsElementModelDependencies _babHairstyleModelDependencies;
        [SerializeField] private CosmeticsElementModelDependencies _pupHairstyleModelDependencies;

        public override void InstallBindings()
        {
            Install(CosmeticsTarget.BabHairstyle, _babHairstyleModelDependencies);
            Install(CosmeticsTarget.PupHairstyle, _pupHairstyleModelDependencies);
        }

        private void Install(CosmeticsTarget target, CosmeticsElementModelDependencies dependencies)
        {
            Container.Bind<CharacterCosmetics>().WithId(target).FromSubContainerResolve()
                .ByMethod(c => InstallCharacter(c, dependencies)).AsCached().NonLazy();
            Container.BindInterfacesTo<CharacterCosmetics>().FromResolve(target).AsCached();
        }

        private static void InstallCharacter(DiContainer container, CosmeticsElementModelDependencies dependencies)
        {
            if (!container.IsValidating)
            {
                dependencies.CosmeticsSave.Load();
                dependencies.CosmeticsCollectionSave.Load();
            }
            container.Bind<ICosmeticsElementModel>().To<CosmeticsElementModel>().AsSingle().WithArguments(
                dependencies.CosmeticsSave.Data.Id,
                dependencies.CosmeticsCollectionSave.Data.IdCollection);
            container.BindInstance(dependencies.Database.I);
            container.Bind<CharacterCosmetics>().AsSingle();
            container.BindInterfacesTo<CosmeticsSave>().FromInstance(dependencies.CosmeticsSave);
            container.BindInterfacesTo<CosmeticsCollectionSave>().FromInstance(dependencies.CosmeticsCollectionSave);
            container.QueueForInject(dependencies.CosmeticsSave);
            container.QueueForInject(dependencies.CosmeticsCollectionSave);
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
