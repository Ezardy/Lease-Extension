using Aniki.UI;
using UnityEngine;
using Zenject;

namespace Aniki.Common {
	[CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common Installer")]
	internal class CommonInstaller : ScriptableObjectInstaller<CommonInstaller> {
		[SerializeField] private LayerNames	layerNames;

		public override void InstallBindings() {
			Container.BindInstance(layerNames);
			Container.BindInterfacesTo<ScreenSizeObserver>().AsSingle();
		}
	}
}