using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Size
{
    internal class ColliderSize : ISize
    {
        private readonly BoxCollider2D _collider;

        public Vector2 Size
        {
            get => _collider.size;
            set => _collider.size = value;
        }
        public Vector3 Min => _collider.bounds.min;

        public ColliderSize(BoxCollider2D collider)
        {
            _collider = collider;
        }
    }
}
