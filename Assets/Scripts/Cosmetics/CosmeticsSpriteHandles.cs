using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Aniki.Cosmetics {
	internal struct CosmeticsSpriteHandles : ICosmeticsSpriteHandles {
		private AsyncOperationHandle<Sprite>	back;
		private AsyncOperationHandle<Sprite>	front;

		public Sprite	Back => back.IsValid() ? back.Result : null;
		public Sprite	Front => front.IsValid() ? front.Result : null;

		public CosmeticsSpriteHandles(AsyncOperationHandle<Sprite> back, AsyncOperationHandle<Sprite> front) {
			this.back = back;
			this.front = front;
		}

		public UniTask	LoadTask() {
			UniTask	awaitable;
			if (back.IsValid() && front.IsValid())
				awaitable = UniTask.WhenAll(back.ToUniTask(), front.ToUniTask());
			else if (back.IsValid())
				awaitable = back.ToUniTask();
			else
				awaitable = front.ToUniTask();
			return awaitable;
		}

		public void	Release() {
			if (back.IsValid())
				back.Release();
			if (front.IsValid())
				front.Release();
		}
	}
}
