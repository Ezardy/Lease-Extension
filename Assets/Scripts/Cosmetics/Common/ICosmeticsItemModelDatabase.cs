using System.Collections.Generic;

namespace Aniki.Cosmetics {
	public interface ICosmeticsItemModelDatabase {
		public IEnumerable<ICosmeticsItemModel>	GetCosmeticsItemModels();
		public ICosmeticsItemModel				GetCosmeticsItemModel(string id);
	}
}
