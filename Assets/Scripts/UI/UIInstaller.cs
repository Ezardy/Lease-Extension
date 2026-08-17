using Aniki.UI;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UI Installer")]
internal class UIInstaller : ScriptableObjectInstaller<UIInstaller> {
	public override void	InstallBindings() {
		Container.BindInterfacesTo<ScreenSizeObserver>().AsSingle();
	}
}