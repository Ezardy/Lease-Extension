using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;

namespace LeaseExtension.Cosmetics.View
{
    [UsedImplicitly]
    internal class CosmeticsElementView : ICosmeticsElementView
    {
        private readonly SpriteRenderer _backRenderer;
        private readonly SpriteRenderer _frontRenderer;

        public CosmeticsElementView(SpriteRenderer backRenderer, SpriteRenderer frontRenderer)
        {
            _backRenderer = backRenderer;
            _frontRenderer = frontRenderer;
        }

        public void Set(Sprite back, Sprite front)
        {
            if (_backRenderer)
                _backRenderer.sprite = back;
            if (_frontRenderer)
                _frontRenderer.sprite = front;
        }
    }
}
