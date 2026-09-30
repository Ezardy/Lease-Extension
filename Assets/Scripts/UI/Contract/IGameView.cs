using UnityEngine;

namespace Aniki.UI {
	internal interface IGameView : IView {
		public void	AddEffect(string id, float duration, Texture texture);
	}
}
