using System;
using LeaseExtension.Common.Contract;
using LeaseExtension.World.Contract;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.World.Floor
{
    [MovedFrom("Aniki.World")]
    internal class FloorView : IFloorView, IInitializable, ITickable, IDisposable
    {
        private readonly ReactiveProperty<float> _perspective = new(0.15f);
        private static readonly int _offsetId = Shader.PropertyToID("_Offset");
        private static readonly int _perspectiveScaleId = Shader.PropertyToID("_Perspective");
        private float _offset = 0;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly IScreenSizeObserver _screenSizeObserver;
        private IDisposable _disposable;

        public float Speed { get; set; }
        public float Perspective
        {
            get => _perspective.Value;
            set => _perspective.Value = value;
        }

        public FloorView(SpriteRenderer spriteRenderer, IScreenSizeObserver screenSizeObserver)
        {
            _spriteRenderer = spriteRenderer;
            _screenSizeObserver = screenSizeObserver;
        }

        public void Tick()
        {
            _offset += Time.deltaTime * Speed;
            _spriteRenderer.material.SetFloat(_offsetId, _offset);
        }

        public void Initialize()
        {
            IDisposable d1 = _screenSizeObserver.SizeChanged.Subscribe(Rescale);
            IDisposable d2 = _perspective.Subscribe(s => _spriteRenderer.material.SetFloat(_perspectiveScaleId, s));
            _disposable = Disposable.Combine(d1, d2);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void Rescale(Vector2Int size)
        {
            float worldWidth = Camera.main.orthographicSize * 2 * size.x / size.y;
            float spriteWidth = _spriteRenderer.bounds.size.x;
            Vector3 scale = _spriteRenderer.transform.localScale;
            scale.x *= worldWidth / spriteWidth;
            _spriteRenderer.transform.localScale = scale;
        }
    }
}
