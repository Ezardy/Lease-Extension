using System;
using LeaseExtension.Common.Contract;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.World.Wall
{
    [MovedFrom("Aniki.World")]
    internal class WallView : IWallView, IInitializable, ITickable, IDisposable
    {
        private static readonly int _offsetId = Shader.PropertyToID("_Offset");
        private readonly SpriteRenderer _renderer;
        private readonly IScreenSizeObserver _screenSizeObserver;
        private IDisposable _disposable;
        private float _offset = 0;

        public float Speed { get; set; }

        public WallView(SpriteRenderer renderer, IScreenSizeObserver screenSizeObserver)
        {
            _renderer = renderer;
            _screenSizeObserver = screenSizeObserver;
        }

        public void Tick()
        {
            _offset += Time.deltaTime * Speed;
            _renderer.material.SetFloat(_offsetId, _offset);
        }

        public void Initialize()
        {
            _disposable = _screenSizeObserver.SizeChanged.Subscribe(s => _renderer.size = new(
                Camera.main.orthographicSize * s.x / s.y * 2 / _renderer.transform.localScale.x,
                _renderer.size.y));
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
