using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace Aniki.Cosmetics {
	public interface ICosmeticsElementModel {
		public string										Id { get; set; }
		public void											AddId(in string hair);
		public IReadOnlyCollection<string>					IdCollection { get; }
		public Observable<string>							IdChanged { get; }
		public Observable<CollectionAddEvent<string>>		IdCollectionChanged { get; }
	}
}
