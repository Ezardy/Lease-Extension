using R3;
using System;
using UnityEngine;

namespace Aniki.UI {
	internal class ScreenSizeObserver : IScreenSizeObserver, IDisposable {
		private readonly IDisposable						disposable;
		private readonly ReactiveProperty<Vector2Int>		size;

		public Vector2Int	Size => size.CurrentValue;

		public Observable<Vector2Int>	SizeChanged => size;

		public ScreenSizeObserver() {
			size = new(new(Screen.width, Screen.height));

			IDisposable	d1 = Observable.EveryValueChanged(this, _ => Screen.width).Subscribe(w => {
				Vector2Int	nsize = size.CurrentValue;

				nsize.x = w;
				size.Value = nsize;
			});
			IDisposable	d2 = Observable.EveryValueChanged(this, _ => Screen.height).Subscribe(h => {
				Vector2Int	nsize = size.CurrentValue;

				nsize.y = h;
				size.Value = nsize;
			});
			disposable = Disposable.Combine(d1, d2);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
