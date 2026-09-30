using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Size
{
    internal class RendererSize : ISize
    {
        private readonly SpriteRenderer _renderer;

        public Vector2 Size
        {
            get => _renderer.size;
            set => _renderer.size = value;
        }
        public Vector3 Min => _renderer.bounds.min;

        public RendererSize(SpriteRenderer renderer)
        {
            _renderer = renderer;
        }
    }
}
