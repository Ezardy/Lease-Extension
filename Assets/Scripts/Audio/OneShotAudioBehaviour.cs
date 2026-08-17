using UnityEngine;
using Zenject;

namespace Aniki.Audio {
	internal class OneShotAudioBehaviour : MonoBehaviour {
		private AudioPlayerParameters	parameters;
		private IOneShotAudioPlayer		player;

		[Inject]
		public void	Init(IOneShotAudioPlayer player, AudioPlayerParameters parameters) {
			this.player = player;
			this.parameters = parameters;
		}

		public void	Play() {
			player.Play(parameters);
		}
	}
}
