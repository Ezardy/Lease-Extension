using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Blank
{
    internal class GapConstructionBlank : IConstructionBlank
    {
        private readonly IObjectPool<GapConstructionBlank> _blankPool;
        private readonly IObjectPool<GapConstructionPart> _partPool;
        private IConstructionBlank _bottomBlank;
        private IConstructionBlank _gapBlank;
        private IConstructionBlank _topBlank;

        public IConstructionPartBlueprint Blueprint { get; }

        public GapConstructionBlank(
            IConstructionPartBlueprint blueprint,
            IObjectPool<GapConstructionBlank> blankPool,
            IObjectPool<GapConstructionPart> partPool)
        {
            this.Blueprint = blueprint;
            _blankPool = blankPool;
            _partPool = partPool;
        }

        public void Populate(IConstructionBlank bottomBlank, IConstructionBlank gapBlank, IConstructionBlank topBlank)
        {
            _bottomBlank = bottomBlank;
            _gapBlank = gapBlank;
            _topBlank = topBlank;
        }

        public void Depopulate()
        {
            _bottomBlank = null;
            _gapBlank = null;
            _topBlank = null;
        }

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            GapConstructionPart part = _partPool.Get();
            part.Populate(
                this,
                _bottomBlank.Construct(position, scale, order),
                _gapBlank.Construct(position, scale, order),
                _topBlank.Construct(position, scale, order));
            WipeOut();
            return part;
        }

        private void WipeOut()
        {
            _blankPool.Release(this);
        }
    }
}
