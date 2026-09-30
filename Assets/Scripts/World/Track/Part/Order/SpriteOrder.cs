using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Order
{
    internal class SpriteOrder : IOrder
    {
        private readonly SpriteRenderer _renderer;

        public int Order
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }

        public SpriteOrder(SpriteRenderer renderer)
        {
            _renderer = renderer;
        }
    }
}
