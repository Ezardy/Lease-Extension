using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aniki.Cosmetics {
	public interface ICosmeticsSpriteHandles {
		public Sprite	Front { get; }
		public Sprite	Back { get; }
		public UniTask	LoadTask();
		public void		Release();
	}
}
