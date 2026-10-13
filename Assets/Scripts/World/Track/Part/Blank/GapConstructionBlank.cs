using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blank
{
    [UsedImplicitly]
    internal class GapConstructionBlank : IConstructionBlank,
        IPoolable<IConstructionPartBlueprint, IConstructionBlank, IConstructionBlank, IConstructionBlank, IMemoryPool>
    {
        private readonly GapConstructionPart.Factory _partFactory;
        private IConstructionPartBlueprint _blueprint;
        private IMemoryPool _pool;
        private IConstructionBlank _bottomBlank;
        private IConstructionBlank _gapBlank;
        private IConstructionBlank _topBlank;

        public GapConstructionBlank(GapConstructionPart.Factory partFactory)
        {
            _partFactory = partFactory;
        }

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            GapConstructionPart part = _partFactory.Create(_blueprint,
                _bottomBlank.Construct(position, scale, order),
                _gapBlank.Construct(position, scale, order),
                _topBlank.Construct(position, scale, order));
            _pool.Despawn(this);
            return part;
        }

        public void OnDespawned()
        {
            _blueprint = null;
            _bottomBlank = null;
            _gapBlank = null;
            _topBlank = null;
            _pool = null;
        }

        public void OnSpawned(IConstructionPartBlueprint blueprint, IConstructionBlank bottomBlank,
            IConstructionBlank gapBlank, IConstructionBlank topBlank, IMemoryPool pool)
        {
            _blueprint = blueprint;
            _bottomBlank = bottomBlank;
            _gapBlank = gapBlank;
            _topBlank = topBlank;
            _pool = pool;
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, IConstructionBlank, IConstructionBlank,
            IConstructionBlank, GapConstructionBlank>
        {}
    }
}