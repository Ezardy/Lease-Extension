using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Blank
{
    internal class ConstructionBlank : IConstructionBlank
    {
        private readonly IObjectPool<ConstructionPart> _partPool;
        private readonly IObjectPool<ConstructionBlank> _blankPool;
        private float _height;
        private float _size;

        public ConstructionBlank(
            IConstructionPartBlueprint blueprint,
            IObjectPool<ConstructionPart> partPool,
            IObjectPool<ConstructionBlank> blankPool)
        {
            _partPool = partPool;
            _blankPool = blankPool;
        }

        public void Populate(float height, float size)
        {
            _height = height;
            _size = size;
        }

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            ConstructionPart instance = _partPool.Get();
            float worldHeight = Camera.main.transform.position.y + Camera.main.orthographicSize - position.y;
            instance.Place(position + worldHeight * _height * Vector2.up, scale, _size * worldHeight, order);
            _blankPool.Release(this);
            return instance;
        }
    }
}
