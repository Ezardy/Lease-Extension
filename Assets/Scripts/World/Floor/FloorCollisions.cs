using Aniki.Common;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World.Floor {
	internal class FloorCollisions : MonoBehaviour {
		private int									playerLayerId;
		private IPublisher<FloorCollisionMessage>	publisher;

		[Inject]
		public void	Init(IPublisher<FloorCollisionMessage> publisher, LayerNames layerNames) {
			this.publisher = publisher;
			playerLayerId = LayerMask.NameToLayer(layerNames.Player);
		}

		private void	OnCollisionEnter2D(Collision2D collision) {
			if (collision.gameObject.layer == playerLayerId)
				publisher.Publish(new());
		}
	}
}
