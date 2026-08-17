using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace Aniki.Cosmetics {
	internal class CosmeticsElementModel : ICosmeticsElementModel {
		private readonly ReactiveProperty<string>	id;
		private readonly ObservableHashSet<string>	idCollection;

		public CosmeticsElementModel(string id, IReadOnlyCollection<string> idCollection) {
			this.id = new(id);
			this.idCollection = new(idCollection);
		}

		public string	Id {
			get => id.CurrentValue;
			set {
				if (idCollection.Contains(value))
					id.Value = value;
			}
		}

		public IReadOnlyCollection<string> IdCollection => idCollection;

		public void	AddId(in string id) {
			idCollection.Add(id);
		}

		public Observable<string>	IdChanged => id;

		public Observable<CollectionAddEvent<string>>	IdCollectionChanged => idCollection.ObserveAdd();
	}
}
