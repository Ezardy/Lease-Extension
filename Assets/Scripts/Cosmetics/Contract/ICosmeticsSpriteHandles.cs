using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aniki.Cosmetics {
	internal interface ICosmeticsSpriteHandles {
		public Sprite	Front { get; }
		public Sprite	Back { get; }
		public UniTask	LoadTask();
		public void		Release();
	}
}
