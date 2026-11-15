using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Service
{
    [UsedImplicitly]
    public class SpriteWidthResizer : RendererWidthResizer
    {
        private readonly SpriteRenderer _renderer;


        public SpriteWidthResizer(SpriteRenderer renderer, Camera camera, IScreenSizeObserver screenSizeObserver,
            [InjectOptional] float width = 1,
            [InjectOptional] float minRatio = 0, [InjectOptional] float maxRatio = float.PositiveInfinity) : base(
            renderer, camera, screenSizeObserver, width, minRatio, maxRatio)
        {
            _renderer = renderer;
        }

        protected override float CalculateInternalWidth(Vector2Int s)
        {
            return Camera.orthographicSize * s.x / s.y * 2 / _renderer.transform.localScale.x;
        }

        protected override void ApplyInternalWidth(float width)
        {
            _renderer.size = new(width, _renderer.size.y);
        }
    }
}