using LeaseExtension.World.Entities.Internal;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Order
{
    internal class TransformOrder : IOrder
    {
        private readonly Transform	transform;

        public int	Order
        {
            get => (int)transform.position.z;
            set
            {
                Vector3	position = transform.position;

                position.z = value;
                transform.position = position;
            }
        }

        public TransformOrder(Transform transform)
        {
            this.transform = transform;
        }
    }
}
