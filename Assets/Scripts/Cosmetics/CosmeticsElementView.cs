using Aniki.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Aniki.Cosmetics {
	internal class CosmeticsElementView<T> : ICosmeticsElementView, ITyped<T> where T : class {
		private readonly SpriteRenderer	backRenderer;
		private readonly SpriteRenderer	frontRenderer;

		public CosmeticsElementView(List<SpriteRenderer> renderers) {
			backRenderer = renderers[0];
			frontRenderer = renderers[1];
		}

		public void	Set(Sprite back, Sprite front) {
			if (back != null)
				backRenderer.sprite = back;
			if (front != null)
				frontRenderer.sprite = front;
		}
	}
}
