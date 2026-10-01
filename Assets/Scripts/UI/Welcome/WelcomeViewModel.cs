using System;
using Cysharp.Threading.Tasks;
using LeaseExtension.Common.Contract;
using LeaseExtension.UI.Contract;
using R3;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LeaseExtension.UI.Welcome
{
    internal class WelcomeViewModel : IWelcomeViewModel, IDisposable
    {
        private readonly AssetReferenceSprite _horizontalWelcome;
        private readonly AssetReferenceSprite _verticalWelcome;
        private readonly IDisposable _disposable;
        private bool _isVertical;
        private AsyncOperationHandle<Sprite> _spriteHandle;

        [CreateProperty]
        public Sprite Welcome { get; private set; }

        public WelcomeViewModel(
            IWelcomeView view,
            AssetReferenceSprite horizontalWelcome,
            AssetReferenceSprite verticalWelcome,
            IScreenSizeObserver screenSizeObserver)
        {
            Vector2Int screenSize = screenSizeObserver.Size;
            _horizontalWelcome = horizontalWelcome;
            _verticalWelcome = verticalWelcome;
            UniTask.WaitWhile(() => view.Root == null).ContinueWith(() => view.Root.dataSource = this).Forget();
            _isVertical = screenSize.x > screenSize.y;
            _disposable = screenSizeObserver.SizeChanged.Subscribe(SetSprite);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            if (_spriteHandle.IsValid())
                _spriteHandle.Release();
        }

        private void SetSprite(Vector2Int size)
        {
            AsyncOperationHandle<Sprite> handle = new();
            if (_isVertical && size.x > size.y)
            {
                handle = _horizontalWelcome.LoadAssetAsync();
                _isVertical = false;
            }
            else if (!_isVertical && size.x <= size.y)
            {
                handle = _verticalWelcome.LoadAssetAsync();
                _isVertical = true;
            }

            if (handle.IsValid())
            {
                if (_spriteHandle.IsValid())
                    _spriteHandle.Release();
                _spriteHandle = handle;
                handle.Completed += OnSpriteLoaded;
            }
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            handle.Completed -= OnSpriteLoaded;
            Welcome = handle.Result;
        }
    }
}
