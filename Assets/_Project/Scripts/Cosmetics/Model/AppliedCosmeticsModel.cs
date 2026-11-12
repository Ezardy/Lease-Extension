using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using R3;

namespace LeaseExtension.Cosmetics.Model
{
    [UsedImplicitly]
    internal class AppliedCosmeticsModel : IAppliedCosmeticsModel
    {
        private readonly ReactiveProperty<string> _id;

        public string Id
        {
            get => _id.CurrentValue;
            set => _id.Value = value;
        }

        public Observable<string> IdChanged => _id;

        public AppliedCosmeticsModel(string id)
        {
            _id = new(id);
        }
    }
}
