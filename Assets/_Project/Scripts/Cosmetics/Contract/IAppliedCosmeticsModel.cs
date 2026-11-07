using R3;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface IAppliedCosmeticsModel
    {
        public string Id { get; set; }
        public Observable<string> IdChanged { get; }
    }
}