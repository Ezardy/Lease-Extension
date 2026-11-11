namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsPreview
    {
        public CosmeticsTarget Target { get; }
        public void SetPreview(ICosmeticsModel cosmetics);
    }
}
