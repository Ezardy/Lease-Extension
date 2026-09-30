using Aniki.Common;
using LeaseExtension.World.Entities.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class Tooth : AConstructionPartBehaviour {
		private IPublisher<ToothPicked>	publisher;
		private int									layer;

		[Inject]
		public void	Init(IPublisher<ToothPicked> publisher,
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
