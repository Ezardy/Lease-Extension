using R3;
using System;
using UnityEngine;

namespace Aniki.World {
	internal class WorldViewModel : IDisposable {
		private readonly IDisposable	disposable;

		public WorldViewModel (IWorldModel model) {
			disposable = model.GravityChanged.Subscribe(g =>
				Physics2D.gravity = new(0, g)
			);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
