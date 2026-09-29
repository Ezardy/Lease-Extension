using Aniki.World;
using UnityEngine;
using UnityEngine.Pool;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class ConstructionPart : IConstructionPart {
        protected readonly IConstructionPartBlueprint		blueprint;
        protected readonly IObjectPool<ConstructionPart>	pool;
        protected readonly Transform						poolTransform;
        protected readonly Transform						transform;
        protected readonly Vector3							initScale;

        public ConstructionPart(IConstructionPartBlueprint blueprint,
            Transform transform,
            IObjectPool<ConstructionPart> pool) {
            this.blueprint = blueprint;
            this.pool = pool;
            this.transform = transform;
            poolTransform = transform.parent;
            initScale = transform.transform.localScale;
        }

        public IConstructionPartBlueprint	Blueprint => blueprint;

        public float X => transform.position.x;

        public virtual void	Place(Vector3 position, float scale, float s, int order) {
            transform.parent = null;
            transform.localScale *= scale;
            transform.position = position;
            transform.gameObject.SetActive(true);
        }

        public void	WipeOut() {
            transform.parent = poolTransform;
            transform.gameObject.SetActive(false);
            transform.localScale = initScale;
            pool.Release(this);
        }

        public void	Dispose() {
            Object.Destroy(transform.gameObject);
        }

        public void	Move(float shift) {
            transform.Translate(shift, 0, 0);
        }
    }
}
