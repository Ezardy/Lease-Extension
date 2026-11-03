using System;
using LeaseExtension.Common.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Service
{
    public class RendererWidthResizer : IInitializable, IDisposable
    {
        protected readonly Camera Camera;
        
        private readonly Renderer _renderer;
        private readonly IScreenSizeObserver _screenSizeObserver;
        private readonly float _width;
        private readonly float _minRatio;
        private readonly float _maxRatio;
        private IDisposable _disposable;

        protected virtual Bounds Bounds => _renderer.bounds;

        public RendererWidthResizer(Renderer renderer, Camera camera, IScreenSizeObserver screenSizeObserver,
            [InjectOptional] float width = 1,
            [InjectOptional] float minRatio = 0, [InjectOptional] float maxRatio = float.PositiveInfinity)
        {
            Camera = camera;
            _renderer = renderer;
            _screenSizeObserver = screenSizeObserver;
            _width = width;
            _minRatio = minRatio;
            _maxRatio = maxRatio;
        }
        
        public void Initialize()
        {
            _disposable = _screenSizeObserver.SizeChanged.Subscribe(s =>
            {
                float height = Bounds.size.y;
                float minWidth = _minRatio * height;
                float maxWidth = _maxRatio * height;
                float internalWidth = CalculateInternalWidth(s) * _width;
                ApplyInternalWidth(internalWidth);
                float width = Bounds.size.x;
                if (width < minWidth)
                {
                    internalWidth *= minWidth / width;
                    ApplyInternalWidth(internalWidth);
                }
                else if (width > maxWidth)
                {
                    internalWidth *= maxWidth / width;
                    ApplyInternalWidth(internalWidth);
                }
            });
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }

        protected virtual float CalculateInternalWidth(Vector2Int s)
        {
            return _renderer.transform.localScale.x / Bounds.size.x * Camera.orthographicSize * 2 * s.x / s.y;
        }

        protected virtual void ApplyInternalWidth(float width)
        {
            Debug.Log(Bounds);
            Vector3 scale = _renderer.transform.localScale;
            scale.x = width;
            _renderer.transform.localScale = scale;
        }
    }
}