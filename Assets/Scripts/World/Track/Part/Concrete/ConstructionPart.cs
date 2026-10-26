using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class ConstructionPart : IConstructionPart, IPoolable<IConstructionPartBlueprint, IMemoryPool>
    {
        protected readonly Transform Transform;
        
        private readonly Transform _poolTransform;
        private readonly Vector3 _initScale;
        private IMemoryPool _pool;

        public IConstructionPartBlueprint Blueprint { get; private set; }
        public float X => Transform.position.x;

        protected ConstructionPart(Transform transform)
        {
            Transform = transform;
            _poolTransform = transform.parent;
            _initScale = transform.localScale;
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

        public void OnDespawned()
        {
            Transform.SetParent(_poolTransform);
            Transform.gameObject.SetActive(false);
            Transform.localScale = _initScale;
            Blueprint = null;
            _pool = null;
        }

        public virtual void OnSpawned(IConstructionPartBlueprint blueprint, IMemoryPool pool)
        {
            Blueprint = blueprint;
            _pool = pool;
        }

        public void Move(float shift)
        {
            Transform.Translate(shift, 0, 0);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprint, ConstructionPart>
        {}
    }
}