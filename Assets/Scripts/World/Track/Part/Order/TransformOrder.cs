using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Order
{
    internal class TransformOrder : IOrder
    {
        private readonly Transform _transform;

        public int Order
        {
            get => (int)_transform.position.z;
            set
            {
                Vector3 position = _transform.position;
                position.z = value;
                _transform.position = position;
            }
        }

        public TransformOrder(Transform transform)
        {
            _transform = transform;
        }
    }
}
