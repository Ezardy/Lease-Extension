using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.View;
using LeaseExtension.Cosmetics.ViewModel;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Installer
{
    [RequireComponent(typeof(GameObjectContext))]
    internal class CosmeticsMonoInstaller : MonoInstaller
    {
        [SerializeField] private CosmeticsTarget _target;
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private SpriteRenderer _frontRenderer;

        public override void InstallBindings()
        {
            Container.Bind<ICosmeticsElementModel>().FromResolveGetter<CharacterCosmetics>(
                _target, c => c.Model).AsCached();
            Container.Bind<ICosmeticsItemModelDatabase>().FromResolveGetter<CharacterCosmetics>(
                _target, c => c.Database).AsCached();
            Container.Bind<ICosmeticsElementView>().To<CosmeticsElementView>().AsSingle()
                .WithArguments(_backRenderer, _frontRenderer);
            Container.BindInterfacesTo<CosmeticsElementViewModel>().AsSingle();
        }
    }
}
