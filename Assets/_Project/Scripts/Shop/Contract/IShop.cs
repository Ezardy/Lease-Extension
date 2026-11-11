using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IShop
    {
        public bool TryAcquire(CosmeticsTarget target, IProductModel product, string productId);
    }
}
