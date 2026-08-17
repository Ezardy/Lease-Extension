using Aniki.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

internal class UIMonoInstaller : MonoInstaller {
	[SerializeField] private PanelRenderer	panelRenderer;

	public override void	InstallBindings() {
		Container.BindInstance(panelRenderer);

		Container.BindInterfacesTo<MainView>().AsSingle();
		Container.BindInterfacesTo<MainViewModel>().AsSingle().NonLazy();

		Container.BindInterfacesTo<OverView>().AsSingle();
		Container.BindInterfacesTo<OverViewModel>().AsSingle();

		Container.BindInterfacesTo<GameView>().AsSingle();
		Container.BindInterfacesTo<GameViewModel>().AsSingle().NonLazy();
	}
}