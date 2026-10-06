using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsSpriteHandles
    {
        public Sprite Front { get; }
        public Sprite Back { get; }

        public UniTask LoadTask();

        public void Release();
    }
}
