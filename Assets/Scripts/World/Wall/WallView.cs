using Aniki.UI;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class WallView : IWallView, IInitializable, ITickable, IDisposable {
		private static readonly int	offsetId = Shader.PropertyToID("_Offset");

		private readonly SpriteRenderer			renderer;
		private readonly IScreenSizeObserver	screenSizeObserver;

		private IDisposable	disposable;

		private float	offset = 0;

		public void	Tick() {
			offset += Time.deltaTime * Speed;
			renderer.material.SetFloat(offsetId, offset);
		}

		public WallView(SpriteRenderer renderer, IScreenSizeObserver screenSizeObserver) {
			this.renderer = renderer;
			this.screenSizeObserver = screenSizeObserver;
		}

		public void	Initialize() {
			disposable = screenSizeObserver.SizeChanged.Subscribe(s =>
				renderer.size = new Vector2(
					Camera.main.orthographicSize * s.x / s.y * 2 / renderer.transform.localScale.x,
					renderer.size.y)
			);
		}

		public float	Speed { get; set; }

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
