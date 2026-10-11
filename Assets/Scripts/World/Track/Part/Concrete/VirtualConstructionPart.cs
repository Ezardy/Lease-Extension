using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using Zenject;

namespace LeaseExtension.World.Track.Part.Concrete
{
    [UsedImplicitly]
    internal class VirtualConstructionPart : IConstructionPart, IPoolable<IConstructionPartBlueprint, IMemoryPool>
    {
        private IMemoryPool _pool;

        public IConstructionPartBlueprint Blueprint { get; private set; }
        public float X { get; private set; } = 0;

        public void OnSpawned(IConstructionPartBlueprint blueprint, IMemoryPool pool)
        {
            Blueprint = blueprint;
            _pool = pool;
        }

        public void OnDespawned()
        {
            Blueprint = null;
            X = 0;
            _pool = null;
        }

        public void Move(float shift)
        {
            X += shift;
        }

        public void WipeOut()
        {
            _pool.Despawn(this);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, VirtualConstructionPart>
        {
        }
    }
}
