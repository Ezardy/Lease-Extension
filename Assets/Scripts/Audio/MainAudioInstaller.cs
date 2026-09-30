using LeaseExtension.Audio.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Contract.Message;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.Audio
{
    [MovedFrom("Aniki.Audio")]
    internal class MainAudioInstaller : MonoInstaller
    {
        [SerializeField]
        [FormerlySerializedAs("punch")]
        private VolumeGeneratorPair _punch;

        [SerializeField]
        [FormerlySerializedAs("obstacle")]
        private VolumeGeneratorPair _obstacle;

        [SerializeField]
        [FormerlySerializedAs("floor")]
        private VolumeGeneratorPair _floor;

        [SerializeField]
        [FormerlySerializedAs("tooth")]
        private VolumeGeneratorPair _tooth;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("sign")]
        private VolumeGeneratorPair _sign;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("weightSwing")]
        private VolumeGeneratorPair _weightSwing;

        [SerializeField]
        [FormerlySerializedAs("swingPlayer")]
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
