using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using R3;

namespace LeaseExtension.Shop.Model
{
    [UsedImplicitly]
    internal class PreviewCosmeticsModel : IAppliedCosmeticsModel
    {
        private readonly ReactiveProperty<string> _id;

        public string Id
        {
            get => _id.CurrentValue;
            set => _id.Value = value;
        }

        public Observable<string> IdChanged => _id;

        public PreviewCosmeticsModel(string id)
        {
            _id = new(id);
        }
    }
}
