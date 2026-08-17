using Aniki.State;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Aniki.SceneManagment {
	[CreateAssetMenu(fileName = "SceneManagmentInstaller", menuName = "Installers/Scene Managment Installer")]
	internal class SceneManagmentInstaller : ScriptableObjectInstaller<SceneManagmentInstaller> {
		[SerializeField] private AssetReference	mainScene;

		public override void	InstallBindings() {
			InstallSceneStates();
		}

		private void	InstallSceneStates() {
			Container.BindInstance(mainScene).WithId(FocusedScene.MAIN);

			Container.BindFactory<FocusedScene, StatePublisher<FocusedScene>, StatePublisher<FocusedScene>.Factory>();
			Container.BindFactory<WelcomeSceneState, WelcomeSceneState.Factory>();
			Container.BindFactory<AsyncOperationHandle<SceneInstance>, MainSceneState, MainSceneState.Factory>().FromPoolableMemoryPool();

			Container.BindInterfacesTo<SceneManagmentContext>().AsSingle();
		}
	}
}