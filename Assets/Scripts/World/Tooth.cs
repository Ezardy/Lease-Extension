using Aniki.Common;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class Tooth : AConstructionPartBehaviour {
		private IPublisher<ToothCollisionMessage>	publisher;
		private int									layer;

		[Inject]
		public void	Init(IPublisher<ToothCollisionMessage> publisher,
			LayerNames layerNames) {
			this.publisher = publisher;
			layer = LayerMask.NameToLayer(layerNames.Player);
		}

		private void	OnTriggerEnter2D(Collider2D collider) {
			if (collider.gameObject.layer == layer) {
				publisher.Publish(new());
				gameObject.SetActive(false);
			}
		}
	}
}
