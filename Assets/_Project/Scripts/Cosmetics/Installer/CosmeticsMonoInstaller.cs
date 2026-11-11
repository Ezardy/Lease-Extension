using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.View;
using LeaseExtension.Cosmetics.ViewModel;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Cosmetics.Installer
{
    [RequireComponent(typeof(GameObjectContext))]
    internal class CosmeticsMonoInstaller : MonoInstaller, ICosmeticsPreview
    {
        [SerializeField] private CosmeticsTarget _target;
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private SpriteRenderer _frontRenderer;

        public CosmeticsTarget Target => _target;

        public void SetPreview(ICosmeticsModel cosmetics)
        {
            if (_backRenderer)
            {
                _backRenderer.sprite = cosmetics.Back;
            }

            if (_frontRenderer)
            {
                _frontRenderer.sprite = cosmetics.Front;
            }
        }

        public override void InstallBindings()
        {
            Container.Bind<IAppliedCosmeticsModel>().FromResolve(_target);
            Container.Bind<ICosmeticsElementView>().To<CosmeticsElementView>().AsSingle()
                .WithArguments(_backRenderer, _frontRenderer);
            Container.BindInterfacesTo<AppliedCosmeticsViewModel>().AsSingle();
        }
    }
}
