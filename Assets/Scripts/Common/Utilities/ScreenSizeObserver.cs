using System;
using LeaseExtension.Common.Contract;
using R3;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Common.Utilities
{
    [MovedFrom("Aniki.UI")]
    internal class ScreenSizeObserver : IScreenSizeObserver, IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly ReactiveProperty<Vector2Int> _size;

        public Vector2Int Size => _size.CurrentValue;
        public Observable<Vector2Int> SizeChanged => _size;

        public ScreenSizeObserver()
        {
            _size = new(new(Screen.width, Screen.height));
            IDisposable d1 = Observable.EveryValueChanged(this, _ => Screen.width).Subscribe(w =>
            {
                Vector2Int nsize = _size.CurrentValue;
                nsize.x = w;
                _size.Value = nsize;
            });
            IDisposable d2 = Observable.EveryValueChanged(this, _ => Screen.height).Subscribe(h =>
            {
                Vector2Int nsize = _size.CurrentValue;
                nsize.y = h;
                _size.Value = nsize;
            });
            _disposable = Disposable.Combine(d1, d2);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
