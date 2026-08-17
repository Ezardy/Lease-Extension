using Cysharp.Threading.Tasks;
using R3;
using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Aniki.UI {
	internal class WelcomeViewModel : IWelcomeViewModel, IDisposable {
		private readonly AssetReferenceSprite	horizontalWelcome;
		private readonly AssetReferenceSprite	verticalWelcome;
		private readonly IDisposable			disposable;

		[CreateProperty] public Sprite	Welcome { get; private set; }

		private bool							isVertical;
		private AsyncOperationHandle<Sprite>	spriteHandle;

		public WelcomeViewModel(IWelcomeView view,
			AssetReferenceSprite horizontalWelcome,
			AssetReferenceSprite verticalWelcome,
			IScreenSizeObserver screenSizeObserver) {
			Vector2Int	screenSize = screenSizeObserver.Size;

			this.horizontalWelcome = horizontalWelcome;
			this.verticalWelcome = verticalWelcome;
			UniTask.WaitWhile(() => view.Root == null)
				.ContinueWith(() => view.Root.dataSource = this).Forget();
			isVertical = screenSize.x > screenSize.y;
			disposable = screenSizeObserver.SizeChanged.Subscribe(SetSprite);
		}

		private void	SetSprite(Vector2Int size) {
			AsyncOperationHandle<Sprite>	handle = new();

			if (isVertical && size.x > size.y) {
				handle = horizontalWelcome.LoadAssetAsync();
				isVertical = false;
			} else if (!isVertical && size.x <= size.y) {
				handle= verticalWelcome.LoadAssetAsync();
				isVertical = true;
			}
			if (handle.IsValid()) {
				if (spriteHandle.IsValid())
					spriteHandle.Release();
				spriteHandle = handle;
				handle.Completed += OnSpriteLoaded;
			}
		}

		private void	OnSpriteLoaded(AsyncOperationHandle<Sprite> handle) {
			handle.Completed -= OnSpriteLoaded;
			Welcome = handle.Result;
		}

		public void	Dispose() {
			disposable.Dispose();
			if (spriteHandle.IsValid())
				spriteHandle.Release();
		}
	}
}
