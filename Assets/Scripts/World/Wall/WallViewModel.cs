using R3;
using System;

namespace Aniki.World {
	internal class WallViewModel : IDisposable {
		private readonly IDisposable	disposable;

		public WallViewModel(IWallView view, IWorldModel model) {
			disposable = model.SpeedChanged.Subscribe(s =>
				view.Speed = s * model.Perspective * 2 / (1 + model.Perspective));
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
