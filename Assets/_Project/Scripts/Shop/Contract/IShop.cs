using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IShop
    {
        public bool IsOwned(CosmeticsTarget target, IProductModel product);
        public bool TryAcquire(CosmeticsTarget target, IProductModel product);
    }
}
