using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.Model;
using LeaseExtension.Cosmetics.Save;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Installer
{
    [CreateAssetMenu(fileName = "CosmeticsInstaller", menuName = "Installers/Cosmetics Installer")]
    internal class CosmeticsInstaller : ScriptableObjectInstaller<CosmeticsInstaller>
    {
        [SerializeField] private Dictionary<CosmeticsTarget, CosmeticsSave> _saves;

        public override void InstallBindings()
        {
            foreach (KeyValuePair<CosmeticsTarget, CosmeticsSave> save in _saves)
            {
                Install(save.Key, save.Value);
            }
        }

        private void Install(CosmeticsTarget target, CosmeticsSave save)
        {
            Container.Bind<CosmeticsElementModelKernel>().WithId(target).FromSubContainerResolve()
                .ByMethod(c => InstallCharacter(c, save)).AsCached().NonLazy();
            Container.BindInterfacesTo<CosmeticsElementModelKernel>().FromResolve(target).AsCached();
            Container.Bind<IAppliedCosmeticsModel>().WithId(target).FromResolveGetter<CosmeticsElementModelKernel>(target, c => c.Model).AsCached();
            Container.Bind<IAppliedProductModel>().WithId(target).FromResolveGetter<CosmeticsElementModelKernel>(target, c => c.Model).AsCached();
        }

        private static void InstallCharacter(DiContainer container, CosmeticsSave save)
        {
            if (!container.IsValidating)
            {
                save.Load();
            }

            container.Bind<IAppliedProductModel>().To<AppliedCosmeticsModel>().AsSingle().WithArguments(save.Data.Id);
            container.Bind<IAppliedCosmeticsModel>().To<IAppliedProductModel>().FromResolve();
            container.Bind<CosmeticsElementModelKernel>().AsSingle();
            container.BindInterfacesTo<CosmeticsSave>().FromInstance(save);
            container.QueueForInject(save);
        }
        
        [UsedImplicitly]
        private class CosmeticsElementModelKernel : Kernel
        {
            public IAppliedProductModel Model { get; }

            public CosmeticsElementModelKernel(IAppliedProductModel model)
            {
                Model = model;
            }
        }
    }
}
