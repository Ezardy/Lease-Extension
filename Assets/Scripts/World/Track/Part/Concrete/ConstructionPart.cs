using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class ConstructionPart : IConstructionPart, IPoolable<IConstructionPartBlueprint, IMemoryPool>
    {
        protected readonly Transform PoolTransform;
        protected readonly Transform Transform;
        protected readonly Vector3 InitScale;
        private IMemoryPool _pool;

        public IConstructionPartBlueprint Blueprint { get; private set; }
        public float X => Transform.position.x;

        public ConstructionPart(Transform transform)
        {
            Transform = transform;
            PoolTransform = transform.parent;
            InitScale = transform.localScale;
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
            _pool.Despawn(this);
        }

        public virtual void OnDespawned()
        {
            Transform.parent = PoolTransform;
            Transform.gameObject.SetActive(false);
            Transform.localScale = InitScale;
            Blueprint = null;
            _pool = null;
        }

        public void OnSpawned(IConstructionPartBlueprint blueprint, IMemoryPool pool)
        {
            Blueprint = blueprint;
            _pool = pool;
        }

        public void Dispose()
        {
            Object.Destroy(Transform.gameObject);
        }

        public void Move(float shift)
        {
            Transform.Translate(shift, 0, 0);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, ConstructionPart>
        {
        }
    }
}