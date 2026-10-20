using System;
using System.Collections.Generic;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.SceneManagement.States;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace LeaseExtension.SceneManagement.Installer
{
    [CreateAssetMenu(fileName = "SceneManagementInstaller", menuName = "Installers/Scene Management Installer")]
    internal class SceneManagementInstaller : ScriptableObjectInstaller<SceneManagementInstaller>
    {
        [SerializeField] private SceneParameters test;
        [SerializeField] private Dictionary<FocusedScene, SceneParameters> _sceneParameters = new();

        public override void InstallBindings()
        {
            InstallSceneParameters();
            InstallSceneStates();
        }
        
        private void InstallSceneParameters()
        {
            foreach (KeyValuePair<FocusedScene, SceneParameters> sceneParameters in _sceneParameters)
            {
                Container.BindInstance(sceneParameters.Value.SceneReference).WithId(sceneParameters.Key);
                Container.BindInstance(sceneParameters.Value.Prefabs).WithId(sceneParameters.Key);
            }
        }

        private void InstallSceneStates()
        {
            Container.Bind<ReactiveProperty<FocusedScene>>().AsSingle();
            Container.Bind<Observable<FocusedScene>>().To<ReactiveProperty<FocusedScene>>().FromResolve();
            Container.BindInstance(SceneManager.GetSceneAt(0));
            Container.BindFactory<WelcomeSceneState, WelcomeSceneState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<AsyncOperationHandle<SceneInstance>, MainSceneState, MainSceneState.Factory>()
                .FromPoolableMemoryPool();
            Container.BindInterfacesTo<SceneManagementContext>().AsSingle();
        }

        [Serializable]
        private class SceneParameters
        {
            [SerializeField] public AssetReference SceneReference;
            public List<Object> Prefabs;
        }
    }
}