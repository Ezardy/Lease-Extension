using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IShopPreviewSpawner
    {
        public void Show(CosmeticsTarget target, IProductModel product);
        public void Clear();
    }
}
