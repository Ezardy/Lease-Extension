using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blank
{
    [UsedImplicitly]
    internal class ConstructionBlank : IConstructionBlank,
        IPoolable<IConstructionPartBlueprint, ConstructionPart.Factory, float, float, IMemoryPool>
    {
        private IConstructionPartBlueprint _blueprint;
        private ConstructionPart.Factory _partFactory;
        private IMemoryPool _pool;
        private float _height;
        private float _size;

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            ConstructionPart instance = _partFactory.Create(_blueprint);
            float worldHeight = Camera.main.transform.position.y + Camera.main.orthographicSize - position.y;
            instance.Place(position + worldHeight * _height * Vector2.up, scale, _size * worldHeight, order);
            _pool.Despawn(this);
            return instance;
        }

        public void OnDespawned()
        {
            _blueprint = null;
            _partFactory = null;
            _pool = null;
            _height = 0;
            _size = 0;
        }

        public void OnSpawned(IConstructionPartBlueprint blueprint, ConstructionPart.Factory partFactory,
            float height, float size, IMemoryPool pool)
        {
            _blueprint = blueprint;
            _partFactory = partFactory;
            _height = height;
            _size = size;
            _pool = pool;
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, ConstructionPart.Factory, float, float,
            ConstructionBlank>
        {}
    }
}