using System;
using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using R3;
using UnityEngine;

namespace LeaseExtension.Common.Utilities
{
    [UsedImplicitly]
    internal class ScreenSizeObserver : IScreenSizeObserver, IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly ReactiveProperty<Vector2Int> _size;

        public Vector2Int Size => _size.CurrentValue;
        public Observable<Vector2Int> SizeChanged => _size;

        public ScreenSizeObserver()
        {
            _size = new(new(Screen.width, Screen.height));
            _disposable = Observable.EveryValueChanged(this, _ => new Vector2Int(Screen.width, Screen.height)).Subscribe(s =>
            {
                _size.Value = s;
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
