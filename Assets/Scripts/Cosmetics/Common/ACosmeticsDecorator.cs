using Aniki.Common;
using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace Aniki.Cosmetics {
	public class BabHairstyle : ACosmeticsDecorator<BabHairstyle> { }
	public class PupHairstyle : ACosmeticsDecorator<PupHairstyle> { }

	public abstract class ACosmeticsDecorator<T> where T : class {
		public class Database : ICosmeticsItemModelDatabase, ITyped<T> {
			private readonly ICosmeticsItemModelDatabase	database;

			public Database(ICosmeticsItemModelDatabase database) {
				this.database = database;
			}

			public IEnumerable<ICosmeticsItemModel>	GetCosmeticsItemModels() {
				return database.GetCosmeticsItemModels();
			}

			public ICosmeticsItemModel	GetCosmeticsItemModel(string id) {
				return database.GetCosmeticsItemModel(id);
			}
		}

		public class ElementModel : ICosmeticsElementModel, ITyped<T> {
			private readonly ICosmeticsElementModel	model;

			public ElementModel(ICosmeticsElementModel model) {
				this.model = model;
			}

			public string	Id { get => model.Id; set => model.Id = value; }

			public IReadOnlyCollection<string>	IdCollection => model.IdCollection;

			public Observable<string>	IdChanged => model.IdChanged;

			public Observable<CollectionAddEvent<string>>	IdCollectionChanged => model.IdCollectionChanged;

			public void	AddId(in string hair) {
				model.AddId(in hair);
			}
		}
	}
}
