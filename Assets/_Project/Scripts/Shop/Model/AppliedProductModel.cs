using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using R3;

namespace LeaseExtension.Shop.Model
{
    [UsedImplicitly]
    internal class AppliedProductModel : IAppliedProductModel
    {
        private readonly IAppliedCosmeticsModel _cosmetics;

        public string Id
        {
            get => _cosmetics.Id;
            set => _cosmetics.Id = value;
        }

        public Observable<string> IdChanged => _cosmetics.IdChanged;

        public AppliedProductModel(IAppliedCosmeticsModel cosmetics)
        {
            _cosmetics = cosmetics;
        }
    }
}
