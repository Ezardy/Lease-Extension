using Aniki.Cosmetics;
using System;
using UnityEngine;
using Zenject;

internal class CosmeticsMonoInstaller : MonoInstaller {
	[SerializeField] private CosmeticsElementViewDependencies	babViewDependencies;
	[SerializeField] private CosmeticsElementViewDependencies	pupViewDependencies;

	public override void	InstallBindings() {
		Container.BindInterfacesTo<CosmeticsElementViewModel<BabHairstyle>>()
			.FromSubContainerResolve().ByMethod(c =>
				Install<CosmeticsElementView<BabHairstyle>, CosmeticsElementViewModel<BabHairstyle>>(c, babViewDependencies))
			.AsCached();
		Container.BindInterfacesTo<CosmeticsElementViewModel<PupHairstyle>>()
			.FromSubContainerResolve().ByMethod(c =>
				Install<CosmeticsElementView<PupHairstyle>, CosmeticsElementViewModel<PupHairstyle>>(c, pupViewDependencies))
			.AsCached();
	}

	private void	Install<V, VM>(DiContainer container, CosmeticsElementViewDependencies viewDeps)
		where V : ICosmeticsElementView where VM : class {
		container.BindInstance(viewDeps.backSpriteRenderer);
		container.BindInstance(viewDeps.frontSpriteRenderer);
		container.BindInterfacesTo<V>().AsSingle();
		container.Bind<VM>().AsSingle();
	}

	[Serializable]
	private class CosmeticsElementViewDependencies {
		public SpriteRenderer	backSpriteRenderer;
		public SpriteRenderer	frontSpriteRenderer;
	}
}