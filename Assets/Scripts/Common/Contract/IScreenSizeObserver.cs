using R3;
using UnityEngine;

namespace LeaseExtension.Common.Contract
{
    public interface IScreenSizeObserver
    {
        public Vector2Int Size { get; }
        public Observable<Vector2Int> SizeChanged { get; }
    }
}
