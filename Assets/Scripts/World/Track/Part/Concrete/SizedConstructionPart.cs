using Aniki.World;
using LeaseExtension.World.Entities.Internal;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class SizedConstructionPart : ConstructionPart
    {
        private readonly ISize	sizer;
        private readonly IOrder	orderer;

        public SizedConstructionPart(IConstructionPartBlueprint blueprint,
            Transform transform, ISize sizer,
            IObjectPool<ConstructionPart> pool, IOrder orderer)
            : base(blueprint, transform, pool)
        {
            this.sizer = sizer;
            this.orderer = orderer;
        }

        public override void Place(Vector3 position, float scale, float s, int order)
        {
            base.Place(position, scale, s, order);
            if (s != 0)
                sizer.Size = new(sizer.Size.x, s / transform.localScale.y);
            transform.position += position - sizer.Min + blueprint.Margin * Vector3.right;
            orderer.Order = order;
        }
    }
}
