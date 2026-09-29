using Aniki.Common;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class Obstacle : AConstructionPartBehaviour {
		private IPublisher<ObstacleCollisionMessage>	publisher;
		private int										layer;

		[Inject]
		public void	Init(IPublisher<ObstacleCollisionMessage> publisher,
			LayerNames layerNames) {
			this.publisher = publisher;
			layer = LayerMask.NameToLayer(layerNames.Player);
		}

		private void	OnTriggerEnter2D(Collider2D collider) {
			if (collider.gameObject.layer == layer)
				publisher.Publish(new());
		}
	}
}
