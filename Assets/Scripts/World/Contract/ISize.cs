using UnityEngine;

namespace LeaseExtension.World.Contract
{
    public interface ISize
    {
        public Vector2 Size { get; set; }
        public Vector3 Min { get; }
    }
}
