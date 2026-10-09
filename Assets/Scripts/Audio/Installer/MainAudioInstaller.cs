using LeaseExtension.Audio.Contract;
using LeaseExtension.Audio.Player;
using LeaseExtension.Audio.Pool;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Contract.Message;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.Audio.Installer
{
    internal class MainAudioInstaller : MonoInstaller
    {
        [SerializeField] private VolumeGeneratorPair _punch;
        [SerializeField] private VolumeGeneratorPair _obstacle;
        [SerializeField] private VolumeGeneratorPair _floor;
        [SerializeField] private VolumeGeneratorPair _tooth;
        [SerializeField, Space] private VolumeGeneratorPair _sign;
        [SerializeField, Space] private VolumeGeneratorPair _weightSwing;
        [SerializeField] private OneShotAudioBehaviourPlayer _swingPlayer;

        public override void InstallBindings()
        {
            InstallCharacterAudio();
            InstallUIAudio();
        }

        private void InstallUIAudio()
        {
            Container.BindInterfacesTo<EventAudioPlayer<RestartRequested>>().AsSingle()
                .WithArguments(_sign.ToParameters());
        }

        private void InstallCharacterAudio()
        {
            Container.BindInterfacesTo<ReactiveAudioPlayer<CharacterState>>().FromSubContainerResolve()
                .ByMethod(InstallCharacterPunchAudio).AsCached();
            Container.BindInterfacesTo<EventAudioPlayer<ObstacleCollided>>().AsSingle()
                .WithArguments(_obstacle.ToParameters());
            Container.BindInterfacesTo<ReactiveAudioPlayer<CharacterState>>().AsCached().WithArguments(
                CharacterState.Over, _floor.ToParameters());
            Container.BindInterfacesTo<EventAudioPlayer<ToothPicked>>().AsSingle().WithArguments(_tooth.ToParameters());
            Container.Bind<AudioPlayerParameters>().FromSubContainerResolve().ByMethod(InstallCharacterSwingAudio)
                .AsCached();
        }

        private void InstallCharacterSwingAudio(DiContainer container)
        {
            container.Decorate<IObjectPool<AudioSource>>().With<ReusingAudioSourcePool>().WithArguments(1);
            container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();
            container.BindInstance(_weightSwing.ToParameters());
            container.QueueForInject(_swingPlayer);
        }

        private void InstallCharacterPunchAudio(DiContainer container)
        {
            container.Decorate<IObjectPool<AudioSource>>().With<ReusingAudioSourcePool>().WithArguments(2);
            container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();
            container.Bind<ReactiveAudioPlayer<CharacterState>>().AsSingle().WithArguments(
                _punch.ToParameters(), CharacterState.Punch);
        }
    }
}