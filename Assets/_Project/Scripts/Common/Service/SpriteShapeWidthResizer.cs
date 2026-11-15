using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using UnityEngine;
using UnityEngine.U2D;

namespace LeaseExtension.Common.Service
{
    [UsedImplicitly]
    public class SpriteShapeWidthResizer : RendererWidthResizer
    {
        private readonly SpriteShapeController _spriteShapeController;

        public SpriteShapeWidthResizer(SpriteShapeController spriteShapeController, Camera camera,
            IScreenSizeObserver screenSizeObserver,
            float width = 1, float minRatio = 0, float maxRatio = float.PositiveInfinity) : base(
            spriteShapeController.spriteShapeRenderer, camera,
            screenSizeObserver, width, minRatio, maxRatio)
        {
            _spriteShapeController = spriteShapeController;
        }

        public override void Initialize()
        {
            _spriteShapeController.BakeMesh().Complete();
            _spriteShapeController.spriteShapeRenderer.SetLocalAABB(
                _spriteShapeController.spriteShapeRenderer.GetBounds()[0]);
            base.Initialize();
        }
    }
}