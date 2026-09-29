using Aniki.World;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Blank
{
    internal class ConstructionBlank : IConstructionBlank {
        private readonly IObjectPool<ConstructionPart>	partPool;
        private readonly IObjectPool<ConstructionBlank>	blankPool;

        private float	height;
        private float	size;

        public ConstructionBlank(IConstructionPartBlueprint blueprint,
            IObjectPool<ConstructionPart> partPool,
            IObjectPool<ConstructionBlank> blankPool) {
            this.partPool = partPool;
            this.blankPool = blankPool;
        }

        public void	Populate(float height, float size) {
            this.height = height;
            this.size = size;
        }

        public IConstructionPart	Construct(Vector2 position, float scale, int order) {
            ConstructionPart	instance = partPool.Get();
            float				worldHeight = Camera.main.transform.position.y
                + Camera.main.orthographicSize - position.y;

            instance.Place(position + worldHeight * height * Vector2.up,
                scale, size * worldHeight, order);
            blankPool.Release(this);
            return instance;
        }
    }
}
