using System;
using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Service
{
    [UsedImplicitly]
    public class SpriteWidthResizer : IInitializable, IDisposable
    {
        protected readonly SpriteRenderer Renderer;
        protected readonly Camera Camera;
        
        private readonly IScreenSizeObserver _screenSizeObserver;
        private readonly float _width;
        private readonly float _minRatio;
        private readonly float _maxRatio;
        private IDisposable _disposable;

        public SpriteWidthResizer(SpriteRenderer renderer, Camera camera, IScreenSizeObserver screenSizeObserver,
            [InjectOptional] float width = 1,
            [InjectOptional] float minRatio = 0, [InjectOptional] float maxRatio = float.PositiveInfinity)
        {
            Renderer = renderer;
            Camera = camera;
            _screenSizeObserver = screenSizeObserver;
            _width = width;
            _minRatio = minRatio;
            _maxRatio = maxRatio;
        }
        
        public void Initialize()
        {
            _disposable = _screenSizeObserver.SizeChanged.Subscribe(s =>
            {
                float minWidth = _minRatio * Renderer.bounds.size.y;
                float maxWidth = _maxRatio * Renderer.bounds.size.y;
                float width = CalculateWidth(s) * _width;
                if (width < minWidth)
                {
                    width = minWidth;
                }
                else if (width > maxWidth)
                {
                    width = maxWidth;
                }
                ApplyWidth(width);
            });
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }

        protected virtual float CalculateWidth(Vector2Int s)
        {
            return Camera.orthographicSize * s.x / s.y * 2 / Renderer.transform.localScale.x;
        }

        protected virtual void ApplyWidth(float width)
        {
            Renderer.size = new(width, Renderer.size.y);
        }
    }
}