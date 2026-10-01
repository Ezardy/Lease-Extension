using LeaseExtension.Audio.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Contract.Message;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.Audio
{
    internal class MainAudioInstaller : MonoInstaller
    {
        [SerializeField]
        private VolumeGeneratorPair _punch;

        [SerializeField]
        private VolumeGeneratorPair _obstacle;

        [SerializeField]
        private VolumeGeneratorPair _floor;

        [SerializeField]
        private VolumeGeneratorPair _tooth;

        [Space]
        [SerializeField]
        private VolumeGeneratorPair _sign;

        [Space]
        [SerializeField]
        private VolumeGeneratorPair _weightSwing;

        [SerializeField]
        private OneShotAudioBehaviour _swingPlayer;

        public override void InstallBindings()
        {
            InstallCharacterAudio();
            InstallUIAudio();
        }

        private void InstallUIAudio()
        {
            Container.BindInterfacesTo<EventAudio<RestartRequested>>().AsSingle().WithArguments(_sign.ToParameters());
        }

        private void InstallCharacterAudio()
        {
            Container.BindInterfacesTo<EventAudio<CharacterState>>().FromSubContainerResolve().ByMethod(InstallCharacterPunchAudio).AsCached();
            Container.BindInterfacesTo<EventAudio<ObstacleCollided>>().AsSingle().WithArguments(_obstacle.ToParameters());
            Container.BindInterfacesTo<EventAudio<CharacterState>>().AsCached().WithArguments(
                _floor.ToParameters(),
                CharacterStateFilter.Over);
            Container.BindInterfacesTo<EventAudio<ToothPicked>>().AsSingle().WithArguments(_tooth.ToParameters());
            Container.Bind<AudioPlayerParameters>().FromSubContainerResolve().ByMethod(InstallCharacterSwingAudio).AsCached();
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
            container.Bind<EventAudio<CharacterState>>().AsSingle().WithArguments(
                CharacterStateFilter.Punch,
                _punch.ToParameters());
        }
    }
}
