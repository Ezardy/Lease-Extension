using Aniki.World;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Blank
{
    internal class GapConstructionBlank : IConstructionBlank {
        private readonly IConstructionPartBlueprint		blueprint;
        private readonly IObjectPool<GapConstructionBlank>	blankPool;
        private readonly IObjectPool<GapConstructionPart>	partPool;

        private IConstructionBlank	bottomBlank;
        private IConstructionBlank	gapBlank;
        private IConstructionBlank	topBlank;

        public IConstructionPartBlueprint	Blueprint => blueprint;

        public GapConstructionBlank(IConstructionPartBlueprint blueprint,
            IObjectPool<GapConstructionBlank> blankPool,
            IObjectPool<GapConstructionPart> partPool) {
            this.blueprint = blueprint;
            this.blankPool = blankPool;
            this.partPool = partPool;
        }

        public void	Populate(IConstructionBlank bottomBlank,
            IConstructionBlank gapBlank,
            IConstructionBlank topBlank) {
            this.bottomBlank = bottomBlank;
            this.gapBlank = gapBlank;
            this.topBlank = topBlank;
        }

        public void	Depopulate() {
            bottomBlank = null;
            gapBlank = null;
            topBlank = null;
        }

        public IConstructionPart	Construct(Vector2 position,
            float scale, int order) {
            GapConstructionPart	part = partPool.Get();

            part.Populate(this, bottomBlank.Construct(position, scale, order),
                gapBlank.Construct(position, scale, order),
                topBlank.Construct(position, scale, order));
            WipeOut();
            return part;
        }

        private void	WipeOut() {
            blankPool.Release(this);
        }
    }
}
