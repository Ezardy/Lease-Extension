using Aniki.World;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class GapConstructionPart : IConstructionPart {
        private readonly IObjectPool<GapConstructionPart>	pool;

        private IConstructionPart				bottomPart;
        private IConstructionPart				gapPart;
        private IConstructionPart				topPart;

        public GapConstructionPart(IConstructionPartBlueprint blueprint,
            IObjectPool<GapConstructionPart> pool) {
            Blueprint = blueprint;
            this.pool = pool;
        }

        public IConstructionPartBlueprint	Blueprint { get; }

        public void	Populate(IConstructionBlank blank,
            IConstructionPart bottomPart, IConstructionPart gapPart,
            IConstructionPart topPart) {
            this.bottomPart = bottomPart;
            this.gapPart = gapPart;
            this.topPart = topPart;
        }

        public void	Depopulate() {
            bottomPart = null;
            gapPart = null;
            topPart = null;
        }

        public float	X => bottomPart.X;

        public void	Move(float shift) {
            bottomPart.Move(shift);
            gapPart.Move(shift);
            topPart.Move(shift);
        }

        public void	WipeOut() {
            bottomPart.WipeOut();
            gapPart.WipeOut();
            topPart.WipeOut();
            pool.Release(this);
        }
    }
}
