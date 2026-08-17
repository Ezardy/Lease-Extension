using Aniki.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject;

internal class WelcomeMonoInstaller : MonoInstaller {
	[SerializeField] private PanelRenderer			panelRenderer;
	[SerializeField] private AssetReferenceSprite	horizontalWelcome;
	[SerializeField] private AssetReferenceSprite	verticalWelcome;

	public override void	InstallBindings() {
		Container.BindInstance(panelRenderer);
		Container.BindInterfacesTo<WelcomeView>().AsSingle();
		Container.BindInterfacesTo<WelcomeViewModel>().AsSingle().WithArguments(horizontalWelcome, verticalWelcome);
	}
}