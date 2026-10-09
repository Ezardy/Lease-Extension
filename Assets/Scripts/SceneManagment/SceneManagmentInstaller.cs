using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.SceneManagment.States;
using LeaseExtension.State;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace LeaseExtension.SceneManagment
{
    [CreateAssetMenu(fileName = "SceneManagmentInstaller", menuName = "Installers/Scene Managment Installer")]
    internal class SceneManagmentInstaller : ScriptableObjectInstaller<SceneManagmentInstaller>
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
            Container.BindInterfacesTo<SceneManagmentContext>().AsSingle();
        }
    }
}