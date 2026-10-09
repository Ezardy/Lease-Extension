using LeaseExtension.Audio.Background;
using LeaseExtension.Audio.Background.States;
using LeaseExtension.Audio.Player;
using LeaseExtension.Audio.Pool;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio.Installer
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
            InstallBackgroundAudio();
        }

        private void InstallBackgroundAudio()
        {
            Container.Bind<Animator>().FromComponentInNewPrefab(_backgroundPrefab.gameObject).AsSingle();
            Container.BindInterfacesTo<BackgroundAudioContext>().AsSingle();
        }
    }
}
