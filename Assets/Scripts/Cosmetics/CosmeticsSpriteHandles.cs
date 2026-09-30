using Cysharp.Threading.Tasks;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LeaseExtension.Cosmetics
{
    internal struct CosmeticsSpriteHandles : ICosmeticsSpriteHandles
    {
        private AsyncOperationHandle<Sprite> _back;
        private AsyncOperationHandle<Sprite> _front;

        public Sprite Back => _back.IsValid() ? _back.Result : null;
        public Sprite Front => _front.IsValid() ? _front.Result : null;

        public CosmeticsSpriteHandles(AsyncOperationHandle<Sprite> back, AsyncOperationHandle<Sprite> front)
        {
            _back = back;
            _front = front;
        }

        public UniTask LoadTask()
        {
            UniTask awaitable;
            if (_back.IsValid() && _front.IsValid())
                awaitable = UniTask.WhenAll(_back.ToUniTask(), _front.ToUniTask());
            else if (_back.IsValid())
                awaitable = _back.ToUniTask();
            else
                awaitable = _front.ToUniTask();
            return awaitable;
        }

        public void Release()
        {
            if (_back.IsValid())
                _back.Release();
            if (_front.IsValid())
                _front.Release();
        }
    }
}
