using LeaseExtension.Audio.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Audio
{
    internal class OneShotAudioBehaviour : MonoBehaviour
    {
        private AudioPlayerParameters _parameters;
        private IOneShotAudioPlayer _player;

        [Inject]
        public void Init(IOneShotAudioPlayer player, AudioPlayerParameters parameters)
        {
            _player = player;
            _parameters = parameters;
        }

        public void Play()
        {
            _player.Play(_parameters);
        }
    }
}
