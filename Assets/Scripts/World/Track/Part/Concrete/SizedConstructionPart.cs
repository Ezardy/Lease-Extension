using LeaseExtension.World.Contract;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class SizedConstructionPart : ConstructionPart
    {
        private readonly ISize _sizer;
        private readonly IOrder _orderer;

        public SizedConstructionPart(
            IConstructionPartBlueprint blueprint,
            Transform transform,
            ISize sizer,
            IObjectPool<ConstructionPart> pool,
            IOrder orderer) : base(blueprint, transform, pool)
        {
            _sizer = sizer;
            _orderer = orderer;
        }

        public override void Place(Vector3 position, float scale, float s, int order)
        {
            base.Place(position, scale, s, order);
            if (s != 0)
                _sizer.Size = new(_sizer.Size.x, s / Transform.localScale.y);
            Transform.position += position - _sizer.Min + PartBlueprint.Margin * Vector3.right;
            _orderer.Order = order;
        }
    }
}
