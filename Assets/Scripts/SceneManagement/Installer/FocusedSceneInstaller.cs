using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.SceneManagement.Contract.Message;
using UnityEngine;
using Zenject;

namespace LeaseExtension.SceneManagement.Installer
{
    [CreateAssetMenu(fileName = "FocusedSceneInstaller", menuName = "Installers/Focused Scene")]
    internal class FocusedSceneInstaller : ScriptableObjectInstaller<FocusedSceneInstaller>
    {
        [SerializeField] private FocusedScene _scene;
        [SerializeField] private List<Object> _prefabs;

        public override void InstallBindings()
        {
            Container.BindInstance(_scene);
            Container.BindFactory<Object, GameObject, PrefabFactory.Factory>().FromFactory<PrefabFactory>();
            if (_prefabs != null && _prefabs.Count > 0)
            {
                Container.BindInstance(_prefabs);
                Container.BindInterfacesTo<FocusedScenePrefabInstantiator>().AsSingle();
            }
        }
    }
}