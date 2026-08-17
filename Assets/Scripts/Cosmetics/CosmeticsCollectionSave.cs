using Aniki.Save;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Aniki.Cosmetics {
	[CreateAssetMenu(fileName = "CosmeticsCollectionSave", menuName = "Scriptable Objects/Saves/Cosmetics Collection")]
	public class CosmeticsCollectionSave : ASave<CosmeticsCollectionSave.CosmeticsCollection, ICosmeticsElementModel>, IInitializable, IDisposable {
		[Serializable]
		public struct CosmeticsCollection {
			public List<string>	idCollection;
		}

		private ICosmeticsElementModel	cosmetics;
		private IDisposable				disposable;

		[Inject]
		public void	Init(ICosmeticsElementModel cosmetics) {
			this.cosmetics = cosmetics;
		}

		public void	Initialize() {
			Init();
			disposable = cosmetics.IdCollectionChanged.Skip(1).Subscribe(e => {
				data.idCollection.Add(e.Value);
				Save();
			});
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
