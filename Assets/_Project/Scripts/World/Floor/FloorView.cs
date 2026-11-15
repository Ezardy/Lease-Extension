using System;
using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Floor
{
    [UsedImplicitly]
    internal class FloorView : IFloorView, IInitializable, ITickable, IDisposable
    {
        private static readonly int _offsetId = Shader.PropertyToID("_Offset");
        private static readonly int _perspectiveScaleId = Shader.PropertyToID("_Perspective");
        private readonly ReactiveProperty<float> _perspective = new(0.15f);
        private readonly SpriteRenderer _spriteRenderer;
        private float _offset;
        private IDisposable _disposable;

        public float Speed { get; set; }

        public float Perspective
        {
            get => _perspective.Value;
            set => _perspective.Value = value;
        }

        public FloorView(Camera camera, SpriteRenderer spriteRenderer, IScreenSizeObserver screenSizeObserver)
        {
            _spriteRenderer = spriteRenderer;
        }

        public void Tick()
        {
            _offset += Time.deltaTime * Speed;
            _spriteRenderer.material.SetFloat(_offsetId, _offset);
        }

        public void Initialize()
        {
            _disposable = _perspective.Subscribe(s => _spriteRenderer.material.SetFloat(_perspectiveScaleId, s));
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}