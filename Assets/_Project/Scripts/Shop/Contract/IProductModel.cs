using LeaseExtension.Cosmetics.Contract;

namespace LeaseExtension.Shop.Contract
{
    public interface IProductModel : ICosmeticsModel
    {
        public string Label { get; }
        public uint Price { get; }
    }
}