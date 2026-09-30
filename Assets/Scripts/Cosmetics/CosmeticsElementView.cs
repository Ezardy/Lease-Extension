using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Cosmetics
{
    [MovedFrom("Aniki.Cosmetics")]
    internal class CosmeticsElementView<T> : ICosmeticsElementView, ITyped<T> where T : class
    {
        private readonly SpriteRenderer _backRenderer;
        private readonly SpriteRenderer _frontRenderer;

        public CosmeticsElementView(List<SpriteRenderer> renderers)
        {
            _backRenderer = renderers[0];
            _frontRenderer = renderers[1];
        }

        public void Set(Sprite back, Sprite front)
        {
            if (back != null)
                _backRenderer.sprite = back;
            if (front != null)
                _frontRenderer.sprite = front;
        }
    }
}
