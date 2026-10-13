using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blank
{
    [UsedImplicitly]
    internal class VirtualConstructionBlank : IConstructionBlank,
        IPoolable<IConstructionPartBlueprint, VirtualConstructionPart.Factory, IMemoryPool>
    {
        private IConstructionPartBlueprint _blueprint;
        private VirtualConstructionPart.Factory _partFactory;
        private IMemoryPool _pool;

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            VirtualConstructionPart part = _partFactory.Create(_blueprint);
            part.Move(position.x);
            _pool.Despawn(this);
            return part;
        }

        public void OnSpawned(IConstructionPartBlueprint blueprint, VirtualConstructionPart.Factory partFactory,
            IMemoryPool pool)
        {
            _blueprint = blueprint;
            _partFactory = partFactory;
            _pool = pool;
        }

        public void OnDespawned()
        {
            _blueprint = null;
            _partFactory = null;
            _pool = null;
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, VirtualConstructionPart.Factory,
            VirtualConstructionBlank>
        {}
    }
}