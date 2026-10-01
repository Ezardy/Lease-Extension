using LeaseExtension.Common.Layer;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    internal class Obstacle : AConstructionPartBehaviour
    {
        private IPublisher<ObstacleCollided> _publisher;
        private int _layer;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == _layer)
                _publisher.Publish(new());
        }

        [Inject]
        public void Init(IPublisher<ObstacleCollided> publisher, LayerNames layerNames)
        {
            _publisher = publisher;
            _layer = LayerMask.NameToLayer(layerNames.Player);
        }
    }
}
