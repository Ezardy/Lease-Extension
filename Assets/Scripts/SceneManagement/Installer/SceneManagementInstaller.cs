using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.SceneManagement.States;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace LeaseExtension.SceneManagement.Installer
{
    [CreateAssetMenu(fileName = "SceneManagementInstaller", menuName = "Installers/Scene Management Installer")]
    internal class SceneManagementInstaller : ScriptableObjectInstaller<SceneManagementInstaller>
    {
        [SerializeField] private AssetReference _mainScene;

        public override void InstallBindings()
        {
            InstallSceneStates();
        }

        private void InstallSceneStates()
        {
            Container.Bind<ReactiveProperty<FocusedScene>>().AsSingle();
            Container.Bind<Observable<FocusedScene>>().To<ReactiveProperty<FocusedScene>>().FromResolve();
            Container.BindInstance(SceneManager.GetSceneAt(0));
            Container.BindInstance(_mainScene).WithId(FocusedScene.Main);
            Container.BindFactory<WelcomeSceneState, WelcomeSceneState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<AsyncOperationHandle<SceneInstance>, MainSceneState, MainSceneState.Factory>()
                .FromPoolableMemoryPool();
            Container.BindInterfacesTo<SceneManagementContext>().AsSingle();
        }
    }
}