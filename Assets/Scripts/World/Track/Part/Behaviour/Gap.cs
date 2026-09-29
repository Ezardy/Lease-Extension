using Aniki.Common;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class Gap : AConstructionPartBehaviour {
		private IPublisher<BarPassedMessage>	publisher;
		private int								layer;

		private void	OnTriggerExit2D(Collider2D collider) {
			if (collider.gameObject.layer == layer)
				publisher.Publish(new());
		}

		[Inject]
		public void	Init(IPublisher<BarPassedMessage> publisher,
			LayerNames layerNames) {
			this.publisher = publisher;
			layer = LayerMask.NameToLayer(layerNames.Player);
		}
	}
}
