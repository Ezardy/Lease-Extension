using Aniki.Cosmetics;
using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.Save {
	[CreateAssetMenu(fileName = "CosmeticsSave", menuName = "Scriptable Objects/Saves/Cosmetics")]
	public class CosmeticsSave : ASave<CosmeticsSave.Cosmetics, ICosmeticsElementModel>, IInitializable, IDisposable {
		[Serializable]
		public struct Cosmetics {
			public string	id;
		}

		private ICosmeticsElementModel	cosmetics;
		private IDisposable				disposable;

		[Inject]
		public void	Init(ICosmeticsElementModel cosmetics) {
			this.cosmetics = cosmetics;
		}

		public void	Initialize() {
			Init();
			disposable = cosmetics.IdChanged.Skip(1).Subscribe(h => {
				data.id = h;
				Save();
			});
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
