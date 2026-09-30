using Aniki.Common;
using LeaseExtension.World.Entities.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class Obstacle : AConstructionPartBehaviour {
		private IPublisher<ObstacleCollided>	publisher;
		private int										layer;

		[Inject]
		public void	Init(IPublisher<ObstacleCollided> publisher,
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
