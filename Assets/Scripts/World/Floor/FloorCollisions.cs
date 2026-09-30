using LeaseExtension.Common.Layer;
using LeaseExtension.World.Contract.Message;
using MessagePipe;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.World.Floor
{
    [MovedFrom("Aniki.World.Floor")]
    internal class FloorCollisions : MonoBehaviour
    {
        private int _playerLayerId;
        private IPublisher<FloorCollided> _publisher;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == _playerLayerId)
                _publisher.Publish(new());
        }

        [Inject]
        public void Init(IPublisher<FloorCollided> publisher, LayerNames layerNames)
        {
            _publisher = publisher;
            _playerLayerId = LayerMask.NameToLayer(layerNames.Player);
        }
    }
}
