using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using UnityEngine;
using UnityEngine.U2D;

namespace LeaseExtension.Common.Service
{
    [UsedImplicitly]
    public class SpriteShapeWidthResizer : RendererWidthResizer
    {
        private readonly SpriteShapeRenderer _renderer;
        
        public SpriteShapeWidthResizer(SpriteShapeRenderer renderer, Camera camera, IScreenSizeObserver screenSizeObserver,
            float width = 1, float minRatio = 0, float maxRatio = float.PositiveInfinity) : base(renderer, camera,
            screenSizeObserver, width, minRatio, maxRatio)
        {
            _renderer = renderer;
        }

        protected override Bounds Bounds => _renderer.GetBounds()[0];
    }
}