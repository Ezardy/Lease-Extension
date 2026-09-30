using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.SceneManagment.States;
using LeaseExtension.State;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.SceneManagment
{
    [CreateAssetMenu(fileName = "SceneManagmentInstaller", menuName = "Installers/Scene Managment Installer")]
    [MovedFrom("Aniki.SceneManagment")]
    internal class SceneManagmentInstaller : ScriptableObjectInstaller<SceneManagmentInstaller>
    {
        [SerializeField]
        [FormerlySerializedAs("mainScene")]
        private AssetReference _mainScene;

        public override void InstallBindings()
        {
            InstallSceneStates();
        }

        private void InstallSceneStates()
        {
            Container.BindInstance(_mainScene).WithId(FocusedScene.Main);
            Container.BindFactory<FocusedScene, StatePublisher<FocusedScene>, StatePublisher<FocusedScene>.Factory>();
            Container.BindFactory<WelcomeSceneState, WelcomeSceneState.Factory>();
            Container.BindFactory<AsyncOperationHandle<SceneInstance>, MainSceneState, MainSceneState.Factory>().FromPoolableMemoryPool();
            Container.BindInterfacesTo<SceneManagmentContext>().AsSingle();
        }
    }
}
