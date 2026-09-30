using LeaseExtension.World.Contract;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class ConstructionPart : IConstructionPart
    {
        protected readonly IConstructionPartBlueprint PartBlueprint;
        protected readonly IObjectPool<ConstructionPart> Pool;
        protected readonly Transform PoolTransform;
        protected readonly Transform Transform;
        protected readonly Vector3 InitScale;

        public IConstructionPartBlueprint Blueprint => PartBlueprint;
        public float X => Transform.position.x;

        public ConstructionPart(
            IConstructionPartBlueprint blueprint,
            Transform transform,
            IObjectPool<ConstructionPart> pool)
        {
            this.PartBlueprint = blueprint;
            this.Pool = pool;
            this.Transform = transform;
            PoolTransform = transform.parent;
            InitScale = transform.transform.localScale;
        }

        public virtual void Place(Vector3 position, float scale, float s, int order)
        {
            Transform.parent = null;
            Transform.localScale *= scale;
            Transform.position = position;
            Transform.gameObject.SetActive(true);
        }

        public void WipeOut()
        {
            Transform.parent = PoolTransform;
            Transform.gameObject.SetActive(false);
            Transform.localScale = InitScale;
            Pool.Release(this);
        }

        public void Dispose()
        {
            Object.Destroy(Transform.gameObject);
        }

        public void Move(float shift)
        {
            Transform.Translate(shift, 0, 0);
        }
    }
}
