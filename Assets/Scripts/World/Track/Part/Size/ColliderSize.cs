using LeaseExtension.World.Entities.Internal;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Size
{
    internal class ColliderSize : ISize
    {
        private readonly BoxCollider2D	collider;

        public Vector2	Size
        {
            get => collider.size;
            set => collider.size = value;
        }

        public Vector3	Min => collider.bounds.min;

        public ColliderSize(BoxCollider2D collider)
        {
            this.collider = collider;
        }
    }
}
