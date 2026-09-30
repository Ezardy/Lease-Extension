using UnityEngine;

namespace LeaseExtension.World.Entities.Internal
{
    public interface ISize
    {
        public Vector2	Size { get; set; }
        public Vector3	Min { get; }
    }
}
