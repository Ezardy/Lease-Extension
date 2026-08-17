using Aniki.UI;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class FloorView : IFloorView, IInitializable, ITickable, IDisposable {
		private readonly ReactiveProperty<float>	perspective = new(0.15f);

		private static readonly int	offsetId = Shader.PropertyToID("_Offset");
		private static readonly int	perspectiveScaleId = Shader.PropertyToID("_Perspective");

		private float	offset = 0;

		public float	Speed { get; set; }

		private readonly SpriteRenderer			spriteRenderer;
		private readonly IScreenSizeObserver	screenSizeObserver;

		private IDisposable	disposable;

		public float	Perspective {
			get => perspective.Value;
			set => perspective.Value = value;
		}

		public void	Tick() {
			offset += Time.deltaTime * Speed;
			spriteRenderer.material.SetFloat(offsetId, offset);
		}

		public FloorView(SpriteRenderer spriteRenderer, IScreenSizeObserver screenSizeObserver) {
			this.spriteRenderer = spriteRenderer;
			this.screenSizeObserver = screenSizeObserver;
		}

		public void	Initialize() {
			IDisposable	d1 = screenSizeObserver.SizeChanged.Subscribe(Rescale);
			IDisposable	d2 = perspective.Subscribe(s => spriteRenderer.material.SetFloat(perspectiveScaleId, s));

			disposable = Disposable.Combine(d1, d2);
		}

		private void	Rescale(Vector2Int size) {
			float	worldWidth = Camera.main.orthographicSize * 2 * size.x / size.y;
			float	spriteWidth = spriteRenderer.bounds.size.x;
			Vector3	scale = spriteRenderer.transform.localScale;

			scale.x *= worldWidth / spriteWidth;
			spriteRenderer.transform.localScale = scale;
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
