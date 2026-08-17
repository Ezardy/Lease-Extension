using Aniki.State;
using UnityEngine;
using Zenject;

namespace Aniki.Audio {
	[CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/Audio Installer")]
	internal class AudioInstaller : ScriptableObjectInstaller<AudioInstaller> {
		[SerializeField] private AudioSource	poolPrefab;
		[SerializeField] private Animator		backgroundPrefab;

		public override void	InstallBindings() {
			Container.BindFactory<AudioSource, AudioSourceFactory>().FromComponentInNewPrefab(poolPrefab);
			Container.BindInterfacesTo<AudioSourcePool>().AsSingle();
			Container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();

			InstallBackgrounAudio();
		}

		private void	InstallBackgrounAudio() {
			Container.BindFactory<WelcomeAudioState, WelcomeAudioState.Factory>();
			Container.BindFactory<IdleAudioState, IdleAudioState.Factory>().FromPoolableMemoryPool();
			Container.BindFactory<RunAudioState, RunAudioState.Factory>().FromPoolableMemoryPool();
			Container.BindFactory<ResultAudioState, ResultAudioState.Factory>().FromPoolableMemoryPool();

			Container.Bind<Animator>().FromComponentInNewPrefab(backgroundPrefab.gameObject).AsSingle();
			Container.BindInterfacesTo<BackgroundAudioContext>().AsSingle();
		}
	}
}