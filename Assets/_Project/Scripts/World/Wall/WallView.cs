using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Wall
{
    [UsedImplicitly]
    internal class WallView : IWallView, ITickable
    {
        private static readonly int _offsetId = Shader.PropertyToID("_Offset");
        private readonly SpriteRenderer _renderer;
        private float _offset;

        public float Speed { get; set; }

        public WallView(SpriteRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Tick()
        {
            _offset += Time.deltaTime * Speed;
            _renderer.material.SetFloat(_offsetId, _offset);
        }
    }
}