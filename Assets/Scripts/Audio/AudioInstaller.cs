using LeaseExtension.Audio.Background;
using LeaseExtension.Audio.Background.States;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio
{
    [CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/Audio Installer")]
    internal class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        [SerializeField] private AudioSource _poolPrefab;
        [SerializeField] private Animator _backgroundPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<AudioSource, AudioSourceFactory>().FromComponentInNewPrefab(_poolPrefab);
            Container.BindInterfacesTo<AudioSourcePool>().AsSingle();
            Container.BindInterfacesTo<OneShotAudioPlayer>().AsSingle();
            InstallBackgrounAudio();
        }

        private void InstallBackgrounAudio()
        {
            Container.BindFactory<WelcomeAudioState, WelcomeAudioState.Factory>();
            Container.BindFactory<IdleAudioState, IdleAudioState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<RunAudioState, RunAudioState.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<ResultAudioState, ResultAudioState.Factory>().FromPoolableMemoryPool();
            Container.Bind<Animator>().FromComponentInNewPrefab(_backgroundPrefab.gameObject).AsSingle();
            Container.BindInterfacesTo<BackgroundAudioContext>().AsSingle();
        }
    }
}
