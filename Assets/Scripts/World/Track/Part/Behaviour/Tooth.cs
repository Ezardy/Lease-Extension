using LeaseExtension.Common.Layer;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    internal class Tooth : AConstructionPartBehaviour
    {
        private IPublisher<ToothPicked> _publisher;
        private int _layer;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == _layer)
            {
                _publisher.Publish(new());
                gameObject.SetActive(false);
            }
        }

        [Inject]
        public void Init(IPublisher<ToothPicked> publisher, LayerNames layerNames)
        {
            _publisher = publisher;
            _layer = LayerMask.NameToLayer(layerNames.Player);
        }
    }
}
