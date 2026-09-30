using Aniki.Common;
using LeaseExtension.World.Entities.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World.Floor {
	internal class FloorCollisions : MonoBehaviour {
		private int									playerLayerId;
		private IPublisher<FloorCollided>	publisher;

		[Inject]
		public void	Init(IPublisher<FloorCollided> publisher, LayerNames layerNames) {
			this.publisher = publisher;
			playerLayerId = LayerMask.NameToLayer(layerNames.Player);
		}

		private void	OnCollisionEnter2D(Collision2D collision) {
			if (collision.gameObject.layer == playerLayerId)
				publisher.Publish(new());
		}
	}
}
