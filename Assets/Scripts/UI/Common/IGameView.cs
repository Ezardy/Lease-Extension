using UnityEngine;

namespace Aniki.UI {
	public interface IGameView : IView {
		public void	AddEffect(string id, float duration, Texture texture);
	}
}
