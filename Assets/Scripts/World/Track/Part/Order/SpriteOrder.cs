using LeaseExtension.World.Entities.Internal;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Order
{
    internal class SpriteOrder : IOrder
    {
        private readonly SpriteRenderer	renderer;

        public int	Order
        {
            get => renderer.sortingOrder;
            set => renderer.sortingOrder = value;
        }

        public SpriteOrder(SpriteRenderer renderer)
        {
            this.renderer = renderer;
        }
    }
}
