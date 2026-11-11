using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IShop
    {
        public void ExchangeOrApply(CosmeticsTarget target, IProductModel product, string productId);
    }
}
