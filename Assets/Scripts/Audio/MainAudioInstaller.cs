using Aniki.Character;
using Aniki.UI;
using Aniki.World;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Aniki.Audio {
	internal class MainAudioInstaller : MonoInstaller {
		[SerializeField] private VolumeGeneratorPair	punch;
		[SerializeField] private VolumeGeneratorPair	obstacle;
		[SerializeField] private VolumeGeneratorPair	floor;
		[SerializeField] private VolumeGeneratorPair	tooth;

		[Space]

		[SerializeField] private VolumeGeneratorPair	sign;

		[Space]

		[SerializeField] private VolumeGeneratorPair	weightSwing;
		[SerializeField] private OneShotAudioBehaviour	swingPlayer;

		public override void	InstallBindings() {
			InstallCharacterAudio();
			InstallUIAudio();
		}

		private void	InstallUIAudio() {
			Container.BindInterfacesTo<EventAudio<AcceptSentenceMessage>>().AsSingle()
				.WithArguments(sign.ToParameters());
		}

		private void	InstallCharacterAudio() {
			Container.BindInterfacesTo<EventAudio<CharacterState>>().FromSubContainerResolve()
				.ByMethod(InstallCharacterPunchAudio).AsCached();
			Container.BindInterfacesTo<EventAudio<ObstacleCollisionMessage>>().AsSingle()
				.WithArguments(obstacle.ToParameters());
			Container.BindInterfacesTo<EventAudio<CharacterState>>().AsCached()
				.WithArguments(floor.ToParameters(), CharacterStateFilter.Over);
			Container.BindInterfacesTo<EventAudio<ToothCollisionMessage>>().AsSingle()
				.WithArguments(tooth.ToParameters());

			Container.Bind<AudioPlayerParameters>().FromSubContainerResolve()
				.ByMethod(InstallCharacterSwingAudio).AsCached();
		}

		private void	InstallCharacterSwingAudio(DiContainer container) {
			container.Decorate<IObjectPool<AudioSource>>().With<ReusingAudioSourcePool>().WithArguments(1);
			container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();
			container.BindInstance(weightSwing.ToParameters());
			container.QueueForInject(swingPlayer);
		}

		private void	InstallCharacterPunchAudio(DiContainer container) {
			container.Decorate<IObjectPool<AudioSource>>().With<ReusingAudioSourcePool>().WithArguments(2);
			container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();
			container.Bind<EventAudio<CharacterState>>().AsSingle().WithArguments(CharacterStateFilter.Punch, punch.ToParameters());
		}
	}
}
