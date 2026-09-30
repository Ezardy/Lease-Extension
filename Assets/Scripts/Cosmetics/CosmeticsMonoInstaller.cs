using System;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Cosmetics.Contract.Target;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Cosmetics
{
    internal class CosmeticsMonoInstaller : MonoInstaller
    {
        [SerializeField]
        [FormerlySerializedAs("babViewDependencies")]
        private CosmeticsElementViewDependencies _babViewDependencies;

        [SerializeField]
        [FormerlySerializedAs("pupViewDependencies")]
        private CosmeticsElementViewDependencies _pupViewDependencies;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CosmeticsElementViewModel<BabHairstyle>>().FromSubContainerResolve().ByMethod(
                c => Install<CosmeticsElementView<BabHairstyle>, CosmeticsElementViewModel<BabHairstyle>>(c, _babViewDependencies)).AsCached();
            Container.BindInterfacesTo<CosmeticsElementViewModel<PupHairstyle>>().FromSubContainerResolve().ByMethod(
                c => Install<CosmeticsElementView<PupHairstyle>, CosmeticsElementViewModel<PupHairstyle>>(c, _pupViewDependencies)).AsCached();
        }

        private void Install<V, VM>(DiContainer container, CosmeticsElementViewDependencies viewDeps)
            where V : ICosmeticsElementView where VM : class
        {
            container.BindInstance(viewDeps.BackSpriteRenderer);
            container.BindInstance(viewDeps.FrontSpriteRenderer);
            container.BindInterfacesTo<V>().AsSingle();
            container.Bind<VM>().AsSingle();
        }

        [Serializable]
        private class CosmeticsElementViewDependencies
        {
            [FormerlySerializedAs("backSpriteRenderer")]
            public SpriteRenderer BackSpriteRenderer;

            [FormerlySerializedAs("frontSpriteRenderer")]
            public SpriteRenderer FrontSpriteRenderer;
        }
    }
}
