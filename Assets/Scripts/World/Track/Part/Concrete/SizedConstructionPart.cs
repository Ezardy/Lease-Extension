using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class SizedConstructionPart : ConstructionPart
    {
        private readonly ISize _sizer;
        private readonly IOrder _orderer;
        private readonly Vector2 _initialSize;

        public SizedConstructionPart(
            Transform transform,
            ISize sizer,
            IOrder orderer) : base(transform)
        {
            _sizer = sizer;
            _orderer = orderer;
            _initialSize = sizer.Size;
        }

        public override void Place(Vector3 position, float scale, float s, int order)
        {
            base.Place(position, scale, s, order);
            if (s != 0)
                _sizer.Size = new(_sizer.Size.x, s / Transform.localScale.y);
            Transform.position += position - _sizer.Min + Blueprint.Margin * Vector3.right;
            _orderer.Order = order;
        }

        public override void OnDespawned()
        {
            _sizer.Size = _initialSize;
            _orderer.Order = 0;
            base.OnDespawned();
        }
    }
}
