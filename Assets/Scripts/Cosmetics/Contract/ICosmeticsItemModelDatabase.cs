using System.Collections.Generic;

namespace Aniki.Cosmetics {
	internal interface ICosmeticsItemModelDatabase {
		public IEnumerable<ICosmeticsItemModel>	GetCosmeticsItemModels();
		public ICosmeticsItemModel				GetCosmeticsItemModel(string id);
	}
}
