using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using Zenject;

namespace LeaseExtension.World.Track.Part.Concrete
{
    [UsedImplicitly]
    internal class GapConstructionPart : IConstructionPart,
        IPoolable<IConstructionPartBlueprint, IConstructionPart, IConstructionPart, IConstructionPart, IMemoryPool>
    {
        private IMemoryPool _pool;
        private IConstructionPart _bottomPart;
        private IConstructionPart _gapPart;
        private IConstructionPart _topPart;

        public IConstructionPartBlueprint Blueprint { get; private set; }
        public float X => _bottomPart.X;

        public void OnSpawned(IConstructionPartBlueprint blueprint, IConstructionPart bottomPart,
            IConstructionPart gapPart, IConstructionPart topPart, IMemoryPool pool)
        {
            Blueprint = blueprint;
            _bottomPart = bottomPart;
            _gapPart = gapPart;
            _topPart = topPart;
            _pool = pool;
        }

        public void OnDespawned()
        {
            _bottomPart.WipeOut();
            _gapPart.WipeOut();
            _topPart.WipeOut();
            Blueprint = null;
            _bottomPart = null;
            _gapPart = null;
            _topPart = null;
            _pool = null;
        }

        public void Move(float shift)
        {
            _bottomPart.Move(shift);
            _gapPart.Move(shift);
            _topPart.Move(shift);
        }

        public void WipeOut()
        {
            _pool.Despawn(this);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, IConstructionPart, IConstructionPart, IConstructionPart, GapConstructionPart>
        {
        }
    }
}
