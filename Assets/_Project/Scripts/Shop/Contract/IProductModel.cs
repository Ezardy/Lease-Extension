using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IProductModel : ICosmeticsModel
    {
        public uint Price { get; }
    }
}