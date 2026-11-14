using System;
using System.Collections.Generic;
using LeaseExtension.Common.Factory;
using LeaseExtension.SceneManagement.Contract.Message;
using LeaseExtension.SceneManagement.States;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace LeaseExtension.SceneManagement.Installer
{
    [CreateAssetMenu(fileName = "SceneManagementInstaller", menuName = "Installers/Scene Management Installer")]
    internal class SceneManagementInstaller : ScriptableObjectInstaller<SceneManagementInstaller>
    {
        [SerializeField] private List<SceneParameters> _sceneParameters;

        public override void InstallBindings()
        {
            Container.BindFactory<Object, GameObject, PrefabFactory.Factory>().FromFactory<PrefabFactory>();
            Dictionary<FocusedScene, SceneParameters> parameters = InstallSceneParameters();
            InstallSceneStates(parameters);
        }

        private Dictionary<FocusedScene, SceneParameters> InstallSceneParameters()
        {
            Dictionary<FocusedScene, SceneParameters> uniqueParameters = new(_sceneParameters.Count);
            foreach (SceneParameters parameters in _sceneParameters)
            {
                uniqueParameters.Add(parameters.Scene, parameters);
            }

            foreach (KeyValuePair<FocusedScene, SceneParameters> sceneParameters in uniqueParameters)
            {
                Container.BindInstance(sceneParameters.Value.SceneReference).WithId(sceneParameters.Key);
            }

            return uniqueParameters;
        }

        private void InstallSceneStates(Dictionary<FocusedScene, SceneParameters> parameters)
        {
            Container.Bind<ReactiveProperty<FocusedScene>>().AsSingle();
            Container.Bind<Observable<FocusedScene>>().To<ReactiveProperty<FocusedScene>>().FromResolve();
            Container.BindInstance(SceneManager.GetSceneAt(0));
            Container.BindFactory<WelcomeSceneState, WelcomeSceneState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<GameSceneState, GameSceneState.Factory>()
                .FromPoolableMemoryPool(pool =>
                    pool.WithArguments<IReadOnlyCollection<Object>>(parameters[FocusedScene.Game]
                        .ConflictingScenePrefabs));
            Container.BindFactory<ShopSceneState, ShopSceneState.Factory>()
                .FromPoolableMemoryPool(pool =>
                    pool.WithArguments<IReadOnlyCollection<Object>>(parameters[FocusedScene.Shop]
                        .ConflictingScenePrefabs));
            Container.BindInterfacesTo<SceneManagementContext>().AsSingle();
        }

        [Serializable]
        private class SceneParameters
        {
            public FocusedScene Scene;
            public AssetReference SceneReference;
            public List<Object> ConflictingScenePrefabs;
        }
    }
}