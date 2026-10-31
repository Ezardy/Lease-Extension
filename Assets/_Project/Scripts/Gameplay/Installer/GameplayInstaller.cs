using LeaseExtension.Gameplay.Contract.Message;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Gameplay.Installer
{
    [CreateAssetMenu(fileName = "GameplayInstaller", menuName = "Installers/Gameplay")]
    internal class GameplayInstaller : ScriptableObjectInstaller<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ReactiveProperty<GameplayState>>().AsSingle();
            Container.Bind<ReadOnlyReactiveProperty<GameplayState>>().To<ReactiveProperty<GameplayState>>()
                .FromResolve();
            Container.Bind<Observable<GameplayState>>().To<ReactiveProperty<GameplayState>>()
                .FromResolve();
        }
    }
}