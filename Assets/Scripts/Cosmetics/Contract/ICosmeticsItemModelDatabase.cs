using System.Collections.Generic;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsItemModelDatabase
    {
        public IEnumerable<ICosmeticsItemModel> GetCosmeticsItemModels();

        public ICosmeticsItemModel GetCosmeticsItemModel(string id);
    }
}
