using LeaseExtension.Common.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Service
{
    public class SpriteWidthRescaler : SpriteWidthResizer
    {
        public SpriteWidthRescaler(SpriteRenderer renderer, Camera camera, IScreenSizeObserver screenSizeObserver,
            [InjectOptional] float width = 1,
            [InjectOptional] float minRatio = 0, [InjectOptional] float maxRatio = float.PositiveInfinity) : base(renderer, camera, screenSizeObserver, width, minRatio, maxRatio)
        {}

        protected override float CalculateWidth(Vector2Int s)
        {
            return Renderer.transform.localScale.x * Camera.orthographicSize * 2 * s.x / s.y / Renderer.bounds.size.x;
        }

        protected override void ApplyWidth(float width)
        {
            Vector3 scale = Renderer.transform.localScale;
            scale.x = width;
            Renderer.transform.localScale = scale;
        }
    }
}