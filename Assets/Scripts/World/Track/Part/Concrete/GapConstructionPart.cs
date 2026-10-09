using LeaseExtension.World.Contract;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class GapConstructionPart : IConstructionPart
    {
        private readonly IObjectPool<GapConstructionPart> _pool;
        private IConstructionPart _bottomPart;
        private IConstructionPart _gapPart;
        private IConstructionPart _topPart;

        public IConstructionPartBlueprint Blueprint { get; }
        public float X => _bottomPart.X;

        public GapConstructionPart(IConstructionPartBlueprint blueprint, IObjectPool<GapConstructionPart> pool)
        {
            Blueprint = blueprint;
            _pool = pool;
        }

        public void Populate(
            IConstructionBlank blank,
            IConstructionPart bottomPart,
            IConstructionPart gapPart,
            IConstructionPart topPart)
        {
            _bottomPart = bottomPart;
            _gapPart = gapPart;
            _topPart = topPart;
        }

        public void Depopulate()
        {
            _bottomPart = null;
            _gapPart = null;
            _topPart = null;
        }

        public void Move(float shift)
        {
            _bottomPart.Move(shift);
            _gapPart.Move(shift);
            _topPart.Move(shift);
        }

        public void WipeOut()
        {
            _bottomPart.WipeOut();
            _gapPart.WipeOut();
            _topPart.WipeOut();
            _pool.Release(this);
        }
    }
}
