using LeaseExtension.World.Entities.Internal;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Size
{
    internal class RendererSize : ISize {
        private readonly SpriteRenderer	renderer;

        public Vector2	Size {
            get => renderer.size;
            set => renderer.size = value;
        }

        public Vector3	Min => renderer.bounds.min;

        public RendererSize(SpriteRenderer renderer) {
            this.renderer = renderer;
        }
    }
}
