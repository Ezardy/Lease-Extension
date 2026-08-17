using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "WorldInstaller", menuName = "Installers/World Installer")]
	internal class WorldInstaller : ScriptableObjectInstaller<WorldInstaller> {
		[SerializeField] private Camera			cameraPrefab;
		[SerializeField] private EventSystem	eventSystemPrefab;

		public override void	InstallBindings() {
			Container.Bind<Camera>().FromComponentInNewPrefab(cameraPrefab.gameObject).AsSingle().NonLazy();
			Container.Bind<EventSystem>().FromComponentInNewPrefab(eventSystemPrefab.gameObject).AsSingle().NonLazy();
		}
	}
}